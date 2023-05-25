using AxibugProtobuf;
using ClientCore.Common;

namespace ClientCore.Manager
{
    public class AppLogin
    {
        public void Login(string Account)
        {
            Protobuf_Login msg = new Protobuf_Login()
            {
                LoginType = 0,
                Account = Account,
            };
            App.networkHelper.SendToServer((int)CommandID.CmdLogin, ProtoBufHelper.Serizlize(msg));
        }
    }
}
