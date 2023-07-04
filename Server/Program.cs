using ServerCore.Manager;

Console.ForegroundColor = ConsoleColor.Green;
ServerManager.InitServer(23846, 23847);

while (true)
{
    string CommandStr = Console.ReadLine();
    string Command = "";
    Command = ((CommandStr.IndexOf(" ") <= 0) ? CommandStr : CommandStr.Substring(0, CommandStr.IndexOf(" ")));
    switch (Command)
    {
        case "list":
            Console.WriteLine("当前在线:" + ServerManager.g_ClientMgr.GetOnlineClientCount());
            break;
        case "tlist":
            Console.WriteLine("当前TcpTunnel在线:" + ServerManager.g_TcpTunnelMgr.GetOnlineClientCount());
            break;
        default:
            Console.WriteLine("未知命令" + CommandStr);
            break;
    }
}