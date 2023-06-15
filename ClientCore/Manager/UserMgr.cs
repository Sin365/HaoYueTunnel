using AxibugProtobuf;
using ClientCore.Common;
using ClientCore.Data;
using ClientCore.Event;
using ClientCore.Network;
using System;
using System.Security.Cryptography;

namespace ClientCore.Manager
{

    public class UserMgr
    {
        public UserInfo MainPlayer { get; private set; }
        public bool bLogin => MainPlayer != null;
        Dictionary<long, UserInfo> DictUID2User = new Dictionary<long, UserInfo>();
        public UserMgr()
        {
            //网络事件注册
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdUserOnlinelist, RecvUserOnlinelist);
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdUserJoin, RecvCmdUserJoin);
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdUserLeave, RecvGetUserLeave);
        }

        public void SetMainPlayer(long uid,string account,int state)
        {
            MainPlayer = new UserInfo()
            {
                State = state,
                NickName = account,
                UID = uid
            };
        }

        public void Send_GetUserList()
        {
            Protobuf_UserList msg = new Protobuf_UserList()
            {
            };
            App.networkMain.SendToServer((int)CommandID.CmdUserOnlinelist, ProtoBufHelper.Serizlize(msg));
        }

        public void RecvUserOnlinelist(byte[] reqData)
        {
            Protobuf_UserList_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_UserList_RESP>(reqData);
            for(int i = 0;i < msg.UserList.Count;i++) 
            {
                UserMiniInfo mi = msg.UserList[i];
                UpdateOrAddUser(mi);
            }
        }
        public void RecvCmdUserJoin(byte[] reqData)
        {
            Protobuf_UserJoin_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_UserJoin_RESP>(reqData);
            UpdateOrAddUser(msg.UserInfo);
        }

        public void RecvGetUserLeave(byte[] reqData)
        {
            Protobuf_UserLeave_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_UserLeave_RESP>(reqData);
            RemoveUser(msg.UID);
        }

        public void UpdateOrAddUser(UserMiniInfo minfo)
        {
            lock (DictUID2User)
            {
                if (!DictUID2User.ContainsKey(minfo.UID))
                {
                    DictUID2User[minfo.UID] = new UserInfo()
                    {
                        UID = minfo.UID,
                        State = minfo.State,
                        NickName = minfo.NickName,
                    };
                }
                else
                {
                    DictUID2User[minfo.UID].State = minfo.State;
                    DictUID2User[minfo.UID].NickName= minfo.NickName;
                }
            }
            //抛出用户加入事件
            EventSystem.Instance.PostEvent(EEvent.UserJoin, minfo.UID);
        }

        public void RemoveUser(long UID)
        {
            bool bflag = false;
            lock (DictUID2User)
            {
                if (DictUID2User.ContainsKey(UID))
                {
                    DictUID2User.Remove(UID);
                    bflag = true;
                }
            }
            if (bflag)
            {
                //抛出用户离开事件
                EventSystem.Instance.PostEvent(EEvent.UserLeave, UID);
            }
        }

        public UserInfo[] GetUserInfo()
        {
            UserInfo[] ulist = new UserInfo[DictUID2User.Count];
            long[] UIDs = DictUID2User.Keys.ToArray();
            for (int i = 0; i < UIDs.Length; i++)
            {
                ulist[i] = DictUID2User[UIDs[i]];
            }
            return ulist;
        }
    }
}
