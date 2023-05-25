﻿using AxibugProtobuf;

namespace ClientCore
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
            App.networkHelper.SendToServer((int)CommandID.CmdLogin, NetBase.Serizlize(msg));
        }
    }
}
