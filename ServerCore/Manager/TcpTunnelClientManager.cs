using AxibugProtobuf;
using ServerCore.Common;
using ServerCore.Event;
using ServerCore.NetWork;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerCore.Manager
{
    public class TCPTunnelClientInfo
    {
        public long UID { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public Socket _Socket { get; set; }
    }


    public class TcpTunnelClientManager
    {
        public TcpTunnelClientManager()
        {
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcptunnelHello, TcpTunnelHello);
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcptunnelDo, TcptunnelDo);
        }

        private Dictionary<long, TCPTunnelClientInfo> _DictUID2Client = new Dictionary<long, TCPTunnelClientInfo>();
        private Dictionary<Socket, TCPTunnelClientInfo> _DictScoket2Client = new Dictionary<Socket, TCPTunnelClientInfo>();

        void AddClient(long UID,Socket _socket)
        {
            IPEndPoint ipEndPoint = (IPEndPoint)_socket.RemoteEndPoint;
            if (!_DictUID2Client.ContainsKey(UID))
            {
                TCPTunnelClientInfo cinfo = new TCPTunnelClientInfo()
                {
                    UID = UID,
                    IP = ipEndPoint.Address.ToString(),
                    Port = ipEndPoint.Port,
                    _Socket = _socket
                };
                _DictScoket2Client[_socket] = cinfo;
            }
            else
            {
                _DictUID2Client[UID].UID = UID;
                _DictUID2Client[UID].IP = ipEndPoint.Address.ToString();
                _DictUID2Client[UID].Port = ipEndPoint.Port;
                _DictUID2Client[UID]._Socket = _socket;

                _DictScoket2Client[_socket] = _DictUID2Client[UID];
            }
        }

        void RemoveClient(long UID)
        {
            if (_DictUID2Client.ContainsKey(UID))
            {
                _DictScoket2Client.Remove(_DictUID2Client[UID]._Socket);
                _DictUID2Client.Remove(UID);
            }
        }

        void RemoveClientBySocket(Socket _socket)
        {
            if (_DictScoket2Client.ContainsKey(_socket))
            {
                _DictUID2Client.Remove(_DictScoket2Client[_socket].UID);
                _DictScoket2Client.Remove(_socket);
            }
        }

        TCPTunnelClientInfo GetClient(long UID)
        {
            if (_DictUID2Client.ContainsKey(UID))
            {
                return _DictUID2Client[UID];
            }
            else
                return null;
        }

        public void TcpTunnelHello(Socket _socket, byte[] reqData)
        {
            ServerManager.g_Log.Debug("收到TcpTunnel 打洞端口Hello");
            Protobuf_TcpTunnel_HellToSev msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpTunnel_HellToSev>(reqData);
            AddClient(msg.UID, _socket);
            Protobuf_TcpTunnel_HellToSev_RESP resp = new Protobuf_TcpTunnel_HellToSev_RESP();
            ClientSend(msg.UID, (int)CommandID.CmdTcptunnelHello, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(resp));
        }

        public void TcptunnelDo(Socket _socket, byte[] reqData)
        {
            ServerManager.g_Log.Debug("收到TcpTunnel 打洞端口Hello");
            Protobuf_TcpTunnel_DoTunnel msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpTunnel_DoTunnel>(reqData);
            TCPTunnelClientInfo Other = GetClient(msg.TargetUID);
            if (Other == null)
            {
                Protobuf_TcpTunnel_DoTunnel_RESP respToErr = new Protobuf_TcpTunnel_DoTunnel_RESP();
                ClientSend(msg.UID, (int)CommandID.CmdTcptunnelDo, (int)ErrorCode.ErrorNotfand, ProtoBufHelper.Serizlize(respToErr));
                return;
            }

            TCPTunnelClientInfo mine = GetClient(msg.UID);
            Protobuf_TcpTunnel_DoTunnel_RESP respToMine = new Protobuf_TcpTunnel_DoTunnel_RESP()
            {
                MyIP = mine.IP,
                MyPort = mine.Port,
                OtherIP = Other.IP,
                OtherPort= Other.Port,
                TargetUID = msg.TargetUID,
            };
            ClientSend(msg.UID, (int)CommandID.CmdTcptunnelHello, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(respToMine));


            Protobuf_TcpTunnel_DoTunnel_RESP respToOther = new Protobuf_TcpTunnel_DoTunnel_RESP()
            {
                MyIP = Other.IP,
                MyPort = Other.Port,
                OtherIP = mine.IP,
                OtherPort = mine.Port,
                TargetUID = msg.UID,
            };
            ClientSend(msg.TargetUID, (int)CommandID.CmdTcptunnelHello, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(respToOther));
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="CMDID"></param>
        /// <param name="ERRCODE"></param>
        /// <param name="data"></param>
        public void ClientSend(long UID, int CMDID, int ERRCODE, byte[] data)
        {
            if (_DictUID2Client.ContainsKey(UID))
            {
                ServerManager.g_SocketTcpTunnelMgr.SendToSocket(_DictUID2Client[UID]._Socket, CMDID, ERRCODE, data);
            }
        }
    }
}
