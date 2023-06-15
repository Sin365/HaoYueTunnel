using AxibugProtobuf;
using Google.Protobuf;
using ServerCore.Common;
using ServerCore.Event;
using ServerCore.NetWork;
using System.Diagnostics;
using System.Net.Sockets;
using System.Timers;

namespace ServerCore.Manager
{
    public class UserManager
    {
        public UserManager()
        {
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdUserOnlinelist, RecvGetUserList);

            //事件
            EventSystem.Instance.RegisterEvent<long>(EEvent.OnUserJoin, OnUserJoin);
            EventSystem.Instance.RegisterEvent<long>(EEvent.OnUserLeave, OnUserLeave);
        }


        #region 事件
        void OnUserJoin(long UID)
        {
            ServerManager.g_Log.Debug($"P2PUserManager->OnUserJoin UID->{UID}");
            SendUserJoin(UID);
        }
        void OnUserLeave(long UID)
        {
            ServerManager.g_Log.Debug($"P2PUserManager->OnUserLeave UID->{UID}");
            SendUserLeave(UID);
        }
        #endregion

        public void RecvGetUserList(Socket _socket, byte[] reqData)
        {
            Protobuf_UserList msg = ProtoBufHelper.DeSerizlize<Protobuf_UserList>(reqData);

            ClientInfo _c = ServerManager.g_ClientMgr.GetClientForSocket(_socket);
            Protobuf_UserList_RESP respData = new Protobuf_UserList_RESP();

            ClientInfo[] cArr = ServerManager.g_ClientMgr.GetOnlineClientList();
            respData.UserCount = cArr.Length;
            for (int i = 0; i < cArr.Length; i++)
            {
                ClientInfo client = cArr[i];
                respData.UserList.Add(new UserMiniInfo()
                {
                    State = client.State,
                    NickName = client.NickName,
                    UID = client.UID,
                });
            }
            ServerManager.g_Log.Debug($"拉取用户列表->{respData.UserCount}个用户");
            ServerManager.g_ClientMgr.ClientSend(_c, (int)CommandID.CmdUserOnlinelist, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(respData));
        }


        public void SendUserJoin(long UID)
        {
            ClientInfo _c = ServerManager.g_ClientMgr.GetClientForUID(UID);
            if (_c == null)
                return;

            Protobuf_UserJoin_RESP resp = new Protobuf_UserJoin_RESP()
            {
                UserInfo = new UserMiniInfo()
                {
                    State = _c.State,
                    NickName = _c.NickName,
                    UID = _c.UID,
                }
            };
            ServerManager.g_ClientMgr.ClientSendALL((int)CommandID.CmdUserJoin, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(resp));
        }

        public void SendUserLeave(long UID)
        {
            Protobuf_UserLeave_RESP resp = new Protobuf_UserLeave_RESP()
            {
                UID = UID,
            };
            ServerManager.g_ClientMgr.ClientSendALL((int)CommandID.CmdUserLeave, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(resp));
        }
    }
}
