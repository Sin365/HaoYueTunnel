using ClientCore;
ClientManager.Init("127.0.0.1", 23846);

while (true)
{
    string CommandStr = Console.ReadLine();
    string Command = "";
    Command = ((CommandStr.IndexOf(" ") <= 0) ? CommandStr : CommandStr.Substring(0, CommandStr.IndexOf(" ")));
    switch (Command)
    {
        case "login":
            StaticComm.login.Login();
            break;
        default:
            Console.WriteLine("未知命令" + CommandStr);
            break;
    }
}