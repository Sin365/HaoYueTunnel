using ClientCore;
using ClientCore.Data;
using ClientCore.Event;

App.Init("127.0.0.1", 23846, 23847);

//注册事件
EventSystem.Instance.RegisterEvent(EEvent.UserLogin, OnUserLogin);
EventSystem.Instance.RegisterEvent(EEvent.TcpTunnelHelloResp, OnTcpTunnelHelloResp);
EventSystem.Instance.RegisterEvent<long>(EEvent.UserJoin, OnUserJoin);
EventSystem.Instance.RegisterEvent<long>(EEvent.UserLeave, OnUserLeave);
EventSystem.Instance.RegisterEvent<string, string>(EEvent.OnChatMsg, OnChatMsg);
EventSystem.Instance.RegisterEvent<long, string>(EEvent.OnP2PChatMsg, OnP2PChatMsg);

while (true)
{
    string CommandStr = Console.ReadLine();
    string Command = "";
    Command = ((CommandStr.IndexOf(" ") <= 0) ? CommandStr : CommandStr.Substring(0, CommandStr.IndexOf(" ")));
    string[] CmdArr = CommandStr.Split(' ');
    switch (Command)
    {
        case "info":
            {
                Console.WriteLine($"IsLogin:{App.userMgr.bLogin}");
                if (App.userMgr.bLogin)
                {
                    Console.WriteLine($"UID:{App.userMgr.MainPlayer.UID}   NickName:{App.userMgr.MainPlayer.NickName}");
                }
            }
            break;
        case "login":
        case "l":
            if (CmdArr.Length < 2)
            {
                Console.WriteLine("缺省用户名");
                return;
            }
            App.login.Login(CmdArr[1]);
            break;
        case "tcphey":
        case "they":
            App.p2ptcp.SendHellToSev();
            break;
        case "tu":
            if (CmdArr.Length < 2)
            {
                Console.WriteLine("缺省用户名");
                return;
            }
            App.p2ptcp.SendDoTunnel(Convert.ToInt64(CmdArr[1]));
            break;
        case "say":
            if (CmdArr.Length < 2)
            {
                Console.WriteLine("缺省参数");
                return;
            }
            App.chat.SendChatMsg(CmdArr[1]);
            break;
        case "tusay":
            if (CmdArr.Length < 3)
            {
                Console.WriteLine("缺省参数");
                return;
            }
            App.p2pChat.SendChatMsg(Convert.ToInt64(CmdArr[1]), CmdArr[2]);
            break;
        case "ulist":
            {
                UserInfo[] ulist = App.userMgr.GetUserInfo();
                Console.WriteLine("User总数"+ ulist.Length);
                for (int i = 0; i < ulist.Length; i++)
                {
                    Console.WriteLine($"[{i}] UID->{ulist[i].UID} | NickName->{ulist[i].NickName} | State->{ulist[i].State}");
                }
            }
            break;
        default:
            Console.WriteLine("未知命令" + CommandStr);
            break;
    }
}

void OnUserLogin()
{
    Console.WriteLine($"[User]登录成功");
    App.userMgr.Send_GetUserList();

    App.clientMgr.ConnectTcpTunnelServer();
}

void OnTcpTunnelHelloResp()
{
    Console.WriteLine($"[TcpTunnel]TcpTunnelHelloResp");
}

void OnUserJoin(long UID)
{
    Console.WriteLine($"[User]用户{UID}上线");
}

void OnUserLeave(long UID)
{
    Console.WriteLine($"[User]用户{UID}下线");
}

void OnChatMsg(string str1, string str2)
{
    Console.WriteLine($"[Chat]{str1}:{str2}");
}

void OnP2PChatMsg(long uid, string str2)
{
    Console.WriteLine($"[P2PChatMsg]{uid}:{str2}");
}
