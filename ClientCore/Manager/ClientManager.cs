namespace ClientCore
{
    public static class ClientManager
    {
        public static void Init(string IP,int port)
        {
            StaticComm.networkHelper = new NetworkHelper();
            StaticComm.login = new AppLogin();
            StaticComm.networkHelper.Init(IP, port);
        }
    }
}
