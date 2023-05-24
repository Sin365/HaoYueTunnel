using AxibugProtobuf;

namespace ClientCore
{
    public class AppLogin
    {
        public void Login()
        {
            Protobuf_Login msg = new Protobuf_Login()
            {
                LoginType = 0,
                //TODO
            };
            StaticComm.networkHelper.SendToServer((int)CommandID.CmdLogin, NetBase.Serizlize(msg));
        }
    }
}
