using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientCore.Manager;
using ClientCore.Network;

namespace ClientCore
{
    public class App
    {
        public static string TokenStr;
        public static long RID = -1;
        public static string IP;
        public static int Port;
        public static LogManager log;
        public static ClientManager clientMgr;
        public static P2PTcp p2ptcp;
        public static P2PChat p2pChat;
        public static P2PFile p2pFile;
        public static NetworkHelper networkMain;
        public static NetworkHelper networkTcp2S;
        public static AppLogin login;
        public static AppChat chat;
        public static UserMgr userMgr;

        public static void Init(string IP, int port, int tcptunnelport)
        {
            log = new LogManager();
            login = new AppLogin();
            chat = new AppChat();
            userMgr = new UserMgr();
            clientMgr = new ClientManager();
            clientMgr.SetIpData(IP, port, tcptunnelport);
            p2ptcp = new P2PTcp();
            p2pChat = new P2PChat();
            p2pFile = new P2PFile();
            networkMain = new NetworkHelper(Enum.ServerType.MainServer);
            networkMain.Init(IP, port);
            //networkTcp2S = new NetworkHelper(Enum.ServerType.TcpTunnelServer);
            //networkTcp2S.Init(IP, tcptunnelport);
        }
    }


}