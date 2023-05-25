﻿using AxibugProtobuf;
using ServerCore.Event;
using System.Net.Sockets;
using System.Timers;

namespace ServerCore.Manager
{
    public class ClientInfo
    {
        public long UID { get; set; }
        public string NickName { get; set; }
        public Socket _socket { get; set; }
        public bool IsOffline { get; set; } = false;
        public DateTime LogOutDT { get; set; }
        public int State { get; set; } = 0;
    }

    public class ClientManager
    {
        private List<ClientInfo> ClientList = new List<ClientInfo>();
        private Dictionary<Socket, ClientInfo> _DictSocketClient = new Dictionary<Socket, ClientInfo>();
        private Dictionary<long, ClientInfo> _DictUIDClient = new Dictionary<long, ClientInfo>();
        private long TestUIDSeed = 0;

        private System.Timers.Timer _ClientCheckTimer;
        private long _RemoveOfflineCacheMin;


        #region 事件


        #endregion

        public void Init(long ticktime, long RemoveOfflineCacheMin)
        {
            //换算成毫秒
            _RemoveOfflineCacheMin = RemoveOfflineCacheMin * 1000;
            _ClientCheckTimer = new System.Timers.Timer();
            _ClientCheckTimer.Interval = ticktime;
            _ClientCheckTimer.AutoReset = true;
            _ClientCheckTimer.Elapsed += new ElapsedEventHandler(ClientCheckClearOffline_Elapsed);
            _ClientCheckTimer.Enabled = true;
        }

        public long GetNextUID()
        {
            return ++TestUIDSeed;
        }

        private void ClientCheckClearOffline_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime CheckDT = DateTime.Now.AddMinutes(-1 * _RemoveOfflineCacheMin);
            ClientInfo[] OfflineClientlist = ClientList.Where(w => w.IsOffline == true && w.LogOutDT < CheckDT).ToArray();

            Console.WriteLine("开始清理离线过久的玩家的缓存");
            for (int i = 0; i < OfflineClientlist.Length; i++)
            {
                //to do 掉线处理
                RemoveClient(OfflineClientlist[i]);
            }
            GC.Collect();
        }


        //通用处理
        #region clientlist 处理

        public ClientInfo JoinNewClient(Protobuf_Login data, Socket _socket)
        {
            //也许这个函数需加lock

            ClientInfo cinfo = GetClientForSocket(_socket);
            //如果连接还在
            if (cinfo != null)
            {
                cinfo.IsOffline = false;
            }
            else
            {
                cinfo = new ClientInfo()
                {
                    UID = GetNextUID(),
                    _socket = _socket,
                    NickName = data.Account,
                    IsOffline = false,
                };
                AddClient(cinfo);
            }
            EventSystem.Instance.PostEvent(EEvent.OnUserJoin, cinfo.UID);
            return cinfo;
        }

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="client"></param>
        void AddClient(ClientInfo clientInfo)
        {
            try
            {
                Console.WriteLine("追加连接玩家 UID=>" + clientInfo.UID + " | " + clientInfo.NickName);
                lock (ClientList)
                {
                    _DictUIDClient.Add(clientInfo.UID, clientInfo);
                    _DictSocketClient.Add(clientInfo._socket, clientInfo);
                    ClientList.Add(clientInfo);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// 清理连接
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(ClientInfo client)
        {
            lock (ClientList)
            {
                if (_DictUIDClient.ContainsKey(client.UID))
                    _DictUIDClient.Remove(client.UID);

                if (_DictSocketClient.ContainsKey(client._socket))
                    _DictSocketClient.Remove(client._socket);

                ClientList.Remove(client);
            }
        }

        public ClientInfo GetClientForUID(long UID)
        {
            return _DictUIDClient.ContainsKey(UID) ? _DictUIDClient[UID] : null;
        }

        public ClientInfo GetClientForSocket(Socket sk)
        {
            return _DictSocketClient.ContainsKey(sk) ? _DictSocketClient[sk] : null;
        }

        /// <summary>
        /// 获取在线玩家
        /// </summary>
        /// <returns></returns>
        public ClientInfo[] GetOnlineClientList()
        {
            return ClientList.Where(w => w.IsOffline == false).ToArray();
        }


        /// <summary>
        /// 设置玩家离线
        /// </summary>
        /// <param name="sk"></param>
        public void SetClientOfflineForSocket(Socket sk)
        {
            if (!_DictSocketClient.ContainsKey(sk))
                return;

            Console.WriteLine("标记玩家UID" + _DictSocketClient[sk].UID + "为离线");
            _DictSocketClient[sk].IsOffline = true;
            _DictSocketClient[sk].LogOutDT = DateTime.Now;
            EventSystem.Instance.PostEvent(EEvent.OnUserLeave, _DictSocketClient[sk].UID);
        }

        public void RemoveClientForSocket(Socket sk)
        {
            if (!_DictSocketClient.ContainsKey(sk))
                return;

            RemoveClient(_DictSocketClient[sk]);
        }

        #endregion

        public void ClientSendALL(int CMDID, int ERRCODE, byte[] data)
        {
            ClientSend(ClientList, CMDID, ERRCODE, data);
        }

        /// <summary>
        /// 给一组用户发送数据
        /// </summary>
        /// <param name="_toclientlist"></param>
        /// <param name="CMDID"></param>
        /// <param name="ERRCODE"></param>
        /// <param name="data"></param>
        public void ClientSend(List<ClientInfo> _toclientlist, int CMDID, int ERRCODE, byte[] data)
        {
            for (int i = 0; i < _toclientlist.Count(); i++)
            {
                if (_toclientlist[i] == null || _toclientlist[i].IsOffline)
                    continue;
                ServerManager.g_SocketMgr.SendToSocket(_toclientlist[i]._socket, CMDID, ERRCODE, data);
            }
        }

        public void ClientSend(Socket _socket, int CMDID, int ERRCODE, byte[] data)
        {
            //Console.WriteLine("发送数据 CMDID->"+ CMDID);
            ServerManager.g_SocketMgr.SendToSocket(_socket, CMDID, ERRCODE, data);
        }

        /// <summary>
        /// 给一个连接发送数据
        /// </summary>
        /// <param name="_c"></param>
        /// <param name="CMDID"></param>
        /// <param name="ERRCODE"></param>
        /// <param name="data"></param>
        public void ClientSend(ClientInfo _c, int CMDID, int ERRCODE, byte[] data)
        {
            if (_c == null || _c.IsOffline)
                return;
            ServerManager.g_SocketMgr.SendToSocket(_c._socket, CMDID, ERRCODE, data);
        }

        public int GetOnlineClientCount()
        {
            return ClientList.Where(w => !w.IsOffline).Count();
        }
    }
}
