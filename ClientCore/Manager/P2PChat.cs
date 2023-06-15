using AxibugProtobuf;
using ClientCore.Common;
using ClientCore.Event;
using ClientCore.Network;

namespace ClientCore.Manager
{
    public class P2PChat
    {
        public P2PChat() 
        {
            //NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdChatmsg, RecvChatMsg);
        }
        public void SendChatMsg(long UID,string ChatMsg)
        {
            Protobuf_TcpP2P_Chat msg = new Protobuf_TcpP2P_Chat()
            {
                ChatMsg = ChatMsg,
            };
            App.clientMgr.SendToTargetSocket(UID,(int)CommandID.CmdTcpP2PChat ,ProtoBufHelper.Serizlize(msg));
        }
    }
}
