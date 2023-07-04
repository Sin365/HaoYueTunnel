using AxibugProtobuf;
using ClientCore.Common;
using ClientCore.Event;
using ClientCore.Network;
using System.Net;
using System.Net.Sockets;

namespace ClientCore.Manager
{
    public class P2PTcp
    {
        bool bRegToSev = false;
        public P2PTcp()
        {
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcptunnelHello, RecvTcptunnelHello);
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcptunnelDo, RecvCmdTcptunnelDo);
        }

        public void SendHellToSev()
        {
            Protobuf_TcpTunnel_HellToSev msg = new Protobuf_TcpTunnel_HellToSev()
            {
                UID = App.userMgr.MainPlayer.UID
            };
            App.networkTcp2S.SendToServer((int)CommandID.CmdTcptunnelHello, ProtoBufHelper.Serizlize(msg));
        }

        public void RecvTcptunnelHello(byte[] reqData)
        {
            Protobuf_TcpTunnel_HellToSev_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpTunnel_HellToSev_RESP>(reqData);
            bRegToSev = true;
            EventSystem.Instance.PostEvent(EEvent.TcpTunnelHelloResp);
        }


        public void SendDoTunnel(long targetUID)
        {
            Protobuf_TcpTunnel_DoTunnel msg = new Protobuf_TcpTunnel_DoTunnel()
            {
                UID = App.userMgr.MainPlayer.UID,
                TargetUID = targetUID
            };
            App.networkTcp2S.SendToServer((int)CommandID.CmdTcptunnelDo, ProtoBufHelper.Serizlize(msg));
        }

        public void RecvCmdTcptunnelDo(byte[] reqData)
        {
            Protobuf_TcpTunnel_DoTunnel_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpTunnel_DoTunnel_RESP>(reqData);
            //TODO 打洞
            Console.WriteLine($"打洞目标信息 {msg.TargetUID} {msg.OtherIP} {msg.OtherPort}");

            int localPort = ((IPEndPoint)App.networkTcp2S.GetClientSocket().LocalEndPoint).Port;

            //断开连接
            App.networkTcp2S.CloseConntect();

            App.clientMgr.ConnectTCPTargetP2P(msg, localPort);
        }

    }
}