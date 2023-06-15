using AxibugProtobuf;
using ClientCore.Common;
using ClientCore.Enum;
using ClientCore.Event;
using ClientCore.Network;
using System.Net.Sockets;
using System.Timers;

namespace ClientCore.Manager
{
    public class ClientManager
    {
        bool bTryReConTcpTunnelServer = false;
        string IP;
        int port;
        int tcptunnelport;
        int LastlocalPort = 0;

        Dictionary<long,NetworkHelper> DictUID2TcpTaret = new Dictionary<long,NetworkHelper>();

        public ClientManager()
        {
            //注册事件
            EventSystem.Instance.RegisterEvent<ServerType, bool>(EEvent.OnSocketConnect, OnSocketConnect);
            EventSystem.Instance.RegisterEvent<ServerType, long>(EEvent.OnSocketDisconnect, OnSocketDisconnect);
        }

        public void SetIpData(string IP, int port, int tcptunnelport)
        {
            this.IP = IP;
            this.port = port;
            this.tcptunnelport = tcptunnelport;
        }

        void OnSocketConnect(ServerType servertype, bool IsOk)
        {
            switch (servertype)
            {
                case ServerType.MainServer:
                    {
                        if (IsOk)
                        {
                            Console.WriteLine("MainServer连接成功");

                        }
                        else
                        {
                            Console.WriteLine("MainServer连接失败");

                        }
                    }
                    break;
                case ServerType.TcpTunnelServer:
                    {
                        bTryReConTcpTunnelServer = false;
                        if (IsOk)
                        {
                            Console.WriteLine("TcpTunnelServer连接成功");
                            App.p2ptcp.SendHellToSev();
                        }
                        else
                        {
                            Console.WriteLine("TcpTunnelServer连接失败");
                        }
                    }
                    break;
                case ServerType.TcpP2PTarget:
                    {
                        if (IsOk)
                        {
                            Console.WriteLine("TcpP2PTarget连接成功");
                        }
                    }
                    break;
            }
        }
        void OnSocketDisconnect(ServerType servertype, long uid)
        {
            switch (servertype)
            {
                case ServerType.MainServer:
                    {
                        Console.WriteLine("MainServer连接断开");
                    }
                    break;
                case ServerType.TcpTunnelServer:
                    {
                        Console.WriteLine("TcpTunnelServer连接断开");
                        Console.WriteLine("TcpTunnelServer,尝试重连");
                        ReConnectTcpTunnelServer();
                    }
                    break;
                case ServerType.TcpP2PTarget:
                    {
                        Console.WriteLine("TcpP2PTarget连接断开");
                        RemoveTargetSocket(uid);
                    }
                    break;
            }
        }

        public void ConnectTcpTunnelServer()
        {
            App.networkTcp2S = new NetworkHelper(Enum.ServerType.TcpTunnelServer);
            App.networkTcp2S.Init(IP, tcptunnelport);
        }

        public void ReConnectTcpTunnelServer()
        {
            //TODO 因TEST 暂不重连
            return;

            if (bTryReConTcpTunnelServer)
                return;
            if (App.networkTcp2S != null && App.networkTcp2S.GetClientSocket() != null && App.networkTcp2S.GetClientSocket().Connected)
                return;
            bTryReConTcpTunnelServer = true;

            Thread thread = new Thread(ReConnectTcpTunnelServerThread);
            thread.IsBackground = true;
            thread.Start();

        }

        private void ReConnectTcpTunnelServerThread()
        {
            Thread.Sleep(1000);
            App.networkTcp2S.Init(IP, tcptunnelport);
        }


        #region P2PTarget 管理
        void AddTargetSocket(NetworkHelper targetSocket)
        {
            DictUID2TcpTaret[targetSocket.mUID] = targetSocket;
        }

        void RemoveTargetSocket(long UID)
        {
            if (DictUID2TcpTaret.ContainsKey(UID))
            {
                DictUID2TcpTaret.Remove(UID);
            }
        }

        NetworkHelper GetTargetSocket(long UID)
        {
            if (DictUID2TcpTaret.ContainsKey(UID))
            {
                return DictUID2TcpTaret[UID];
            }
            return null;
        }
        
        /// <summary>
        /// 发送给指定UID消息
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="CMDID"></param>
        /// <param name="data"></param>
        public void SendToTargetSocket(long UID,int CMDID,byte[] data)
        {
            NetworkHelper target = GetTargetSocket(UID);
            if (target == null)
                return;
            target.SendToServer((int)CMDID, data);
        }
        #endregion


        #region TCP打洞对连

        public void ConnectTCPTargetP2P(Protobuf_TcpTunnel_DoTunnel_RESP msg, int localPort)
        {
            LastlocalPort = localPort;
            Thread thread = new Thread(ConnectTCPTargetP2PThread);
            thread.IsBackground = true;
            thread.Start(msg);
        }

        public void ConnectTCPTargetP2PThread(object obj)
        {
            Thread.Sleep(1000);
            int userBindPort = LastlocalPort;
            Protobuf_TcpTunnel_DoTunnel_RESP msg = (Protobuf_TcpTunnel_DoTunnel_RESP)obj;
            Console.WriteLine("LocalEndPoint Port：" + userBindPort);
            NetworkHelper targetSocket = new NetworkHelper(Enum.ServerType.TcpP2PTarget, msg.TargetUID);
            targetSocket.Init(msg.OtherIP, msg.OtherPort, true, userBindPort);
            //尝试5次连接
            for (int j = 0; j < 5; j++)
            {
                try
                {
                    targetSocket.Connect(msg.OtherIP, msg.OtherPort);
                    Console.WriteLine("Connect：成功{0},{1}", msg.OtherIP, msg.OtherPort);
                    AddTargetSocket(targetSocket);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Connect：失败");
                }
            }
        }

        #endregion
    }
}
