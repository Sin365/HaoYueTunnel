using ServerCore.Event;
using ServerCore.NetWork;
using System.Net;

namespace ServerCore.Manager
{
    public static class ServerManager
    {
        public static ClientManager g_ClientMgr;
        public static LogManager g_Log;
        public static LoginManager g_Login;
        public static ChatManager g_Chat;
        public static P2PUserManager g_P2PMgr;
        public static TcpTunnelClientManager g_TcpTunnelMgr;
        public static IOCPNetWork g_SocketMgr;
        public static IOCPNetWork g_SocketTcpTunnelMgr;

        public static void InitServer(int port, int tcptunnelport)
        {
            g_ClientMgr = new ClientManager();
            g_Log = new LogManager();
            g_Login = new LoginManager();
            g_Chat = new ChatManager();
            g_P2PMgr = new P2PUserManager();
            g_TcpTunnelMgr = new TcpTunnelClientManager();
            g_SocketMgr = new IOCPNetWork(1024, 1024);
            g_SocketMgr.Init();
            Console.WriteLine("监听:" + port);
            g_SocketMgr.Start(new IPEndPoint(IPAddress.Any.Address, port));
            g_SocketTcpTunnelMgr = new IOCPNetWork(1024, 1024);
            g_SocketTcpTunnelMgr.Init();
            g_SocketTcpTunnelMgr.Start(new IPEndPoint(IPAddress.Any.Address, tcptunnelport));
            Console.WriteLine("监听:" + tcptunnelport);
            Console.WriteLine("Succeed!");
        }
    }
}