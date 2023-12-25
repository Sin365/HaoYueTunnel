using HaoYueNet.ServerNetwork;
using ServerCore.Common.Enum;
using ServerCore.Event;
using ServerCore.Manager;
using System.Net;

namespace ServerCore.NetWork
{
    public class IOCPNetWork : TcpSaeaServer
    {
        ServerType mServerType;
        public IOCPNetWork(int numConnections, int receiveBufferSize, ServerType serverType)
            : base(numConnections, receiveBufferSize)
        {
            mServerType = serverType;
            OnClientNumberChange += ClientNumberChange;
            OnReceive += ReceiveData;
            OnDisconnected += OnDisconnect;
            OnNetLog += OnShowNetLog;
        }

        void ClientNumberChange(int num, AsyncUserToken token)
        {
            Console.WriteLine("Client数发生变化");
        }

        /// <summary>
        /// 接受包回调
        /// </summary>
        /// <param name="CMDID">协议ID</param>
        /// <param name="ERRCODE">错误编号</param>
        /// <param name="data">业务数据</param>
        void ReceiveData(AsyncUserToken token, int CMDID, byte[] data)
        {
            //DataCallBackToOld(token.Socket, CMDID, data);
            ServerManager.g_Log.Debug("收到消息 CMDID =>" + CMDID + " 数据长度=>" + data.Length);
            //抛出网络数据
            NetMsg.Instance.PostNetMsgEvent(CMDID, token.Socket, data);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sk"></param>
        void OnDisconnect(AsyncUserToken token)
        {
            ServerManager.g_Log.Debug($"断开连接,ServerType->{mServerType} | {((IPEndPoint)token.Socket.LocalEndPoint).Address}");
            //ServerManager.g_ClientMgr.SetClientOfflineForSocket(token.Socket);
            //要删除不同的
            EventSystem.Instance.PostEvent(EEvent.OnSocketDisconnect, mServerType, token.Socket);
        }


        void OnShowNetLog(string msg)
        {
            ServerManager.g_Log.Debug(msg);
        }
    }
}
