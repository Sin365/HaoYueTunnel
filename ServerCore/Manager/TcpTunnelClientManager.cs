using AxibugProtobuf;
using ServerCore.Common;
using ServerCore.Common.Enum;
using ServerCore.Event;
using ServerCore.NetWork;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerCore.Manager
{
    public class TCPTunnelClientInfo
    {
        public long UID { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public Socket _Socket { get; set; }
    }


    public class TcpTunnelClientManager
    {
        private System.Timers.Timer _ClientCheckTimer;
        private long _RemoveOfflineCacheMin;
        public TcpTunnelClientManager()
        {
            //消息注册
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcptunnelHello, TcpTunnelHello);
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcptunnelDo, TcptunnelDo);
            //事件注册
            EventSystem.Instance.RegisterEvent<ServerType, Socket>(EEvent.OnSocketDisconnect, OnSocketDisconnect);
        }

        private Dictionary<long, TCPTunnelClientInfo> _DictUID2Client = new Dictionary<long, TCPTunnelClientInfo>();
        private Dictionary<Socket, TCPTunnelClientInfo> _DictScoket2Client = new Dictionary<Socket, TCPTunnelClientInfo>();

        #region 事件
        void OnSocketDisconnect(ServerType serverType, Socket socket)
        {
            if (serverType != ServerType.TcpTunnelServer)
                return;

            RemoveClientBySocket(socket);
        }
        #endregion
        void AddClient(long UID,Socket _socket)
        {
            IPEndPoint ipEndPoint = (IPEndPoint)_socket.RemoteEndPoint;
            if (!_DictUID2Client.ContainsKey(UID))
            {
                TCPTunnelClientInfo cinfo = new TCPTunnelClientInfo()
                {
                    UID = UID,
                    IP = ipEndPoint.Address.ToString(),
                    Port = ipEndPoint.Port,
                    _Socket = _socket
                };
                _DictScoket2Client[_socket] = cinfo;
                _DictUID2Client[UID] = cinfo;
            }
            else
            {
                _DictUID2Client[UID].UID = UID;
                _DictUID2Client[UID].IP = ipEndPoint.Address.ToString();
                _DictUID2Client[UID].Port = ipEndPoint.Port;
                _DictUID2Client[UID]._Socket = _socket;

                _DictScoket2Client[_socket] = _DictUID2Client[UID];
            }
        }

        void RemoveClient(long UID)
        {
            if (_DictUID2Client.ContainsKey(UID))
            {
                _DictScoket2Client.Remove(_DictUID2Client[UID]._Socket);
                _DictUID2Client.Remove(UID);
            }
        }

        void RemoveClientBySocket(Socket _socket)
        {
            if (_DictScoket2Client.ContainsKey(_socket))
            {
                RemoveClient(_DictScoket2Client[_socket].UID);
            }
        }

        TCPTunnelClientInfo GetClient(long UID)
        {
            if (_DictUID2Client.ContainsKey(UID))
            {
                return _DictUID2Client[UID];
            }
            else
                return null;
        }

        public void TcpTunnelHello(Socket _socket, byte[] reqData)
        {
            ServerManager.g_Log.Debug("收到TcpTunnel 打洞端口Hello");
            Protobuf_TcpTunnel_HellToSev msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpTunnel_HellToSev>(reqData);
            AddClient(msg.UID, _socket);
            Protobuf_TcpTunnel_HellToSev_RESP resp = new Protobuf_TcpTunnel_HellToSev_RESP();
            ClientSend(msg.UID, (int)CommandID.CmdTcptunnelHello, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(resp));
        }

        public void TcptunnelDo(Socket _socket, byte[] reqData)
        {
            ServerManager.g_Log.Debug("收到TcpTunnel 打洞端口Hello");
            Protobuf_TcpTunnel_DoTunnel msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpTunnel_DoTunnel>(reqData);
            TCPTunnelClientInfo Other = GetClient(msg.TargetUID);

            if (Other == null || msg.UID == msg.TargetUID)
            {
                Protobuf_TcpTunnel_DoTunnel_RESP respToErr = new Protobuf_TcpTunnel_DoTunnel_RESP();
                ClientSend(msg.UID, (int)CommandID.CmdTcptunnelDo, (int)ErrorCode.ErrorNotfind, ProtoBufHelper.Serizlize(respToErr));
                return;
            }

            //发给自己
            TCPTunnelClientInfo mine = GetClient(msg.UID);
            Protobuf_TcpTunnel_DoTunnel_RESP respToMine = new Protobuf_TcpTunnel_DoTunnel_RESP()
            {
                MyIP = mine.IP,
                MyPort = mine.Port,
                OtherIP = Other.IP,
                OtherPort= Other.Port,
                TargetUID = msg.TargetUID,
            };
            ClientSend(msg.UID, (int)CommandID.CmdTcptunnelDo, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(respToMine));

            //发给对方
            Protobuf_TcpTunnel_DoTunnel_RESP respToOther = new Protobuf_TcpTunnel_DoTunnel_RESP()
            {
                MyIP = Other.IP,
                MyPort = Other.Port,
                OtherIP = mine.IP,
                OtherPort = mine.Port,
                TargetUID = msg.UID,
            };
            ClientSend(msg.TargetUID, (int)CommandID.CmdTcptunnelDo, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(respToOther));

            //TODO 暂时服务器不断开，交由客户端收到后主动断开

            //断开两边
            //mine._Socket.Close();
            //TCPTunnelClientInfo other = GetClient(msg.TargetUID);
            //other._Socket.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="CMDID"></param>
        /// <param name="ERRCODE"></param>
        /// <param name="data"></param>
        public void ClientSend(long UID, int CMDID, int ERRCODE, byte[] data)
        {
            if (_DictUID2Client.ContainsKey(UID))
            {
                ServerManager.g_SocketTcpTunnelMgr.SendToSocket(_DictUID2Client[UID]._Socket, CMDID, ERRCODE, data);
            }
        }

        public int GetOnlineClientCount()
        {
            return _DictUID2Client.Count();
        }
    }
}
