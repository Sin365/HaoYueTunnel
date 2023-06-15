using AxibugProtobuf;
using ClientCore.Common;
using ClientCore.Event;
using ClientCore.Network;

namespace ClientCore.Manager
{
    public class AppLogin
    {
        public AppLogin()
        {
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdLogin, RecvCmdLogin);
        }

        public void Login(string Account)
        {
            Protobuf_Login msg = new Protobuf_Login()
            {
                LoginType = 0,
                Account = Account,
            };
            App.networkMain.SendToServer((int)CommandID.CmdLogin, ProtoBufHelper.Serizlize(msg));
        }

        public void RecvCmdLogin(byte[] reqData)
        {
            Protobuf_Login_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_Login_RESP>(reqData);
            App.userMgr.SetMainPlayer(msg.UID, msg.NickName, 0);
            EventSystem.Instance.PostEvent(EEvent.UserLogin);
        }
    }
}
