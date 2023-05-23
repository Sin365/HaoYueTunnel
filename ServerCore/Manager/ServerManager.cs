﻿using System.Net;

namespace ServerCore
{
    public static class ServerManager
    {
        public static ClientManager g_ClientMgr;
        public static LogManager g_Log;
        public static LoginManager g_Login;
        public static IOCPNetWork g_SocketMgr;

        public static void InitServer(int port)
        {
            g_ClientMgr = new ClientManager();
            g_Log = new LogManager();
            g_Login = new LoginManager();
            g_SocketMgr = new IOCPNetWork(1024, 1024);
            g_SocketMgr.Init();
            g_SocketMgr.Start(new IPEndPoint(IPAddress.Any.Address, port));
            Console.WriteLine("监听:" + port);
            Console.WriteLine("Succeed!");
        }
    }
}