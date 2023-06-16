using AxibugProtobuf;
using ClientCore.Common;
using ClientCore.Event;
using ClientCore.Network;
using Google.Protobuf;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientCore.Manager
{
    public class P2PFile
    {
        public int cPackSize = 5 * 1024;
        public int WaitSendPackCount = 100;
        //是否自动同意接收
        public bool bAutoRecv = true;
        public P2PFile()
        {
            //发送者协议注册
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcpP2PFilepushConfirmResp, RecvTcpP2PFilepushConfirmResp);
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcpP2PFilepushResp, RecvTcpP2PFilepushResp);
            //接收者协议注册
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcpP2PFilepushConfirm, RecvTcpP2PFilepushConfirm);
            NetMsg.Instance.RegNetMsgEvent((int)CommandID.CmdTcpP2PFilepush, RecvTcpP2PFilepush);
        }


        #region 发送者网络消息
        public void SendFilePushConfirm(long UID, FilePushTask taskinfo)
        {
            Protobuf_TcpP2P_FilePushConfirm msg = new Protobuf_TcpP2P_FilePushConfirm()
            {
                TaskID = taskinfo.TaskID,
                PackSize = taskinfo.PackSize,
                ContentType = taskinfo.ContentType,
                FileHash = taskinfo.FileHash,
                FileLenght = taskinfo.FileLenght,
                FileName = taskinfo.FileName,
                PackCount = taskinfo.PackCount,
            };
            App.clientMgr.SendToTargetSocket(UID, (int)CommandID.CmdTcpP2PFilepushConfirm, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(msg));
        }
        public void SendFilePush(long UID,int TaskID, long Index, byte[] data, int dataLength)
        {
            Protobuf_TcpP2P_FilePush msg = new Protobuf_TcpP2P_FilePush()
            {
                TaskID = TaskID,
                PackIndex = Index,
                PackLenght= dataLength,
                FileData = ByteString.CopyFrom(data)
            };
            App.clientMgr.SendToTargetSocket(UID, (int)CommandID.CmdTcpP2PFilepush, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(msg));
        }
        void RecvTcpP2PFilepushConfirmResp(long uid, byte[] reqData)
        {
            Protobuf_TcpP2P_FilePushConfirm_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpP2P_FilePushConfirm_RESP>(reqData);
            // 开始发送
            if (msg.Agree == 1)
            {
                sender_AgreeRecvFile(msg.TaskID);
            }
            else
            {
                //TODO 被拒绝
            }
        }
        void RecvTcpP2PFilepushResp(long uid, byte[] reqData)
        {
            //收到回执信息
            Protobuf_TcpP2P_FilePush_RESP msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpP2P_FilePush_RESP>(reqData);
            sender_StartPushFileThread(msg.TaskID, msg.FinishIndex);
        }
        #endregion

        #region 发送者逻辑
        Dictionary<int, FilePushTask> DictTaskID2Task_SendReady = new Dictionary<int, FilePushTask>();
        Dictionary<int, FilePushTask> DictTaskID2Task_SendDo = new Dictionary<int, FilePushTask>();

        /// <summary>
        /// 发送者发送文件请求
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="path"></param>
        public void sender_FilePushConfirmToTarget(long UID, string path)
        {
            FileInfo finfo = new FileInfo(path);
            //向上取整
            long packCount = (long)Math.Ceiling(finfo.Length / (float)cPackSize);
            FilePushTask ftask = new FilePushTask()
            {
                TaskID = 1,//TODO
                fromUID = App.userMgr.MainPlayer.UID,
                targetUID = UID,
                Path = path,
                FileName = finfo.Name,
                PackSize = cPackSize,
                PackCount = packCount,
                FileLenght = finfo.Length,
                FileHash = "",//TODO
                ContentType = "",//TODO

                CurrIndex = -1,
                ConfirmIndex= -1,
            };
            SendFilePushConfirm(UID, ftask);

            DictTaskID2Task_SendReady[ftask.TaskID] = ftask;
        }
        public void sender_AgreeRecvFile(int TaskID)
        {
            if (DictTaskID2Task_SendReady.ContainsKey(TaskID))
            {
                FilePushTask task = DictTaskID2Task_SendReady[TaskID];
                DictTaskID2Task_SendDo[TaskID] = task;
                DictTaskID2Task_SendReady.Remove(TaskID);
                sender_StartPushFile(TaskID);
            }
        }

        //开始发送文件传输
        void sender_StartPushFile(int TaskID)
        {
            if (!DictTaskID2Task_SendDo.ContainsKey(TaskID))
            {
                return;
            }

            FilePushTask task = DictTaskID2Task_SendDo[TaskID];
            Thread thread = new Thread(sender_StartPushFileThread);
            thread.IsBackground = true;
            thread.Start(TaskID);
        }

        void sender_StartPushFileThread(int TaskID,long Index)
        {
            if (!DictTaskID2Task_SendDo.ContainsKey(TaskID))
                return;
            DictTaskID2Task_SendDo[TaskID].ConfirmIndex = Index;
        }

        /// <summary>
        /// 开始发送线程用
        /// </summary>
        /// <param name="obj"></param>
        void sender_StartPushFileThread(object obj)
        {
            int TaskID = (int)obj;
            if (!DictTaskID2Task_SendDo.ContainsKey(TaskID))
                return;

            FilePushTask task = DictTaskID2Task_SendDo[TaskID];

            //构造读取文件流对象
            using (FileStream fsRead = new FileStream(task.Path, FileMode.Open)) //打开文件，不能创建新的
            {
                //开辟临时缓存内存
                byte[] byteArrayRead = new byte[cPackSize]; // 1字节*1024 = 1k 1k*1024 = 1M内存

                App.log.Debug($"[文件发送者]开始发送文件!!");
                //通过死缓存去读文本中的内容
                while (true)
                {
                    //若发送下标，大于确认下标一定数量，则等待
                    while (task.CurrIndex - task.ConfirmIndex > WaitSendPackCount)
                    {
                        Thread.Sleep(10);
                    }

                    //readCount 这个是保存真正读取到的字节数
                    int readCount = fsRead.Read(byteArrayRead, 0, byteArrayRead.Length);

                    //TODO 大小
                    //if (readCount == cPackSize)
                    //{
                    //}
                    //else
                    //{
                    //}

                    task.CurrIndex++;
                    SendFilePush(task.targetUID, TaskID, task.CurrIndex, byteArrayRead, readCount);
                    //App.log.Debug($"发送文件片段，下标->{task.CurrIndex}");

                    //既然是死循环 那么什么时候我们停止读取文本内容 我们知道文本最后一行的大小肯定是小于缓存内存大小的
                    if (readCount < byteArrayRead.Length)
                    {
                        App.log.Debug($"[文件发送者]文件发送完毕!! task.CurrIndex->{task.CurrIndex}");
                        DictTaskID2Task_SendDo.Remove(TaskID);
                        fsRead.Close();
                        break; //结束循环
                    }
                }
            }
        }
        #endregion

        #region 接收者网络消息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="TaskID"></param>
        /// <param name="bAgree">[1]同意 [0]拒绝</param>
        public void SendFilePushConfirm_Resp(long UID, int TaskID, int bAgree)
        {
            Protobuf_TcpP2P_FilePushConfirm_RESP msg = new Protobuf_TcpP2P_FilePushConfirm_RESP()
            {
                TaskID = TaskID,
                Agree = bAgree
            };
            App.clientMgr.SendToTargetSocket(UID, (int)CommandID.CmdTcpP2PFilepushConfirmResp, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(msg));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="TaskID"></param>
        /// <param name="Index"></param>
        public void SendFilePush_Resp(long UID, int TaskID, long Index)
        {
            Protobuf_TcpP2P_FilePush_RESP msg = new Protobuf_TcpP2P_FilePush_RESP()
            {
                TaskID = TaskID,
                FinishIndex = Index
            };
            App.clientMgr.SendToTargetSocket(UID, (int)CommandID.CmdTcpP2PFilepushResp, (int)ErrorCode.ErrorOk, ProtoBufHelper.Serizlize(msg));
        }
        void RecvTcpP2PFilepushConfirm(long uid, byte[] reqData)
        {
            Protobuf_TcpP2P_FilePushConfirm msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpP2P_FilePushConfirm>(reqData);
            receiver_AddRecvFileReady(new FilePushTask()
            {
                TaskID = msg.TaskID,
                PackSize = cPackSize,
                ContentType = msg.ContentType,
                FileHash = msg.FileHash,
                FileLenght = msg.FileLenght,
                FileName = msg.FileName,
                PackCount = msg.PackCount,

                Path = "",//待插入
                fromUID = uid,
                targetUID = App.userMgr.MainPlayer.UID,
                CurrIndex = -1,
                ConfirmIndex= -1,//虽然用不上
            });
            //TODO 事件 EventSystem.Instance.PostEvent(EEvent.OnP2PChatMsg, uid, msg.ChatMsg);
        }
        void RecvTcpP2PFilepush(long uid, byte[] reqData)
        {
            Protobuf_TcpP2P_FilePush msg = ProtoBufHelper.DeSerizlize<Protobuf_TcpP2P_FilePush>(reqData);
            if (!DictTaskID2Task_RecvDataQueue.ContainsKey(msg.TaskID))
                return;

            if (msg == null)
            {
            }

            DictTaskID2Task_RecvDataQueue[msg.TaskID].Enqueue(msg);
        }

        #endregion

        #region 接受者逻辑
        Dictionary<int, FilePushTask> DictTaskID2Task_RecvReady = new Dictionary<int, FilePushTask>();
        Dictionary<int, FilePushTask> DictTaskID2Task_RecvDo = new Dictionary<int, FilePushTask>();
        Dictionary<int, Queue<Protobuf_TcpP2P_FilePush>> DictTaskID2Task_RecvDataQueue = new Dictionary<int, Queue<Protobuf_TcpP2P_FilePush>>();
        /// <summary>
        /// 接收数据准备
        /// </summary>
        /// <param name="task"></param>
        void receiver_AddRecvFileReady(FilePushTask task)
        {
            DictTaskID2Task_RecvReady[task.TaskID] = task;

            //自动接收文件
            if (bAutoRecv)
            {
                receiver_AgreeRecvFile(task.TaskID);
            }
        }

        /// <summary>
        /// 同意文件传输
        /// </summary>
        /// <param name="TaskID"></param>
        public void receiver_AgreeRecvFile(int TaskID)
        {
            if (DictTaskID2Task_RecvReady.ContainsKey(TaskID))
            {
                FilePushTask task = DictTaskID2Task_RecvReady[TaskID];
                DictTaskID2Task_RecvDo[TaskID] = task;
                DictTaskID2Task_RecvReady.Remove(TaskID);
                receiver_StartRecvFile(TaskID);
                App.log.Debug("[文件接收者]：发送同意接收");
                SendFilePushConfirm_Resp(task.fromUID, task.TaskID, 1);
            }
        }

        /// <summary>
        /// 开始接收文件传输
        /// </summary>
        /// <param name="TaskID"></param>
        void receiver_StartRecvFile(int TaskID)
        {
            if (!DictTaskID2Task_RecvDo.ContainsKey(TaskID))
            {
                return;
            }

            FilePushTask task = DictTaskID2Task_RecvDo[TaskID];
            DictTaskID2Task_RecvDataQueue[TaskID] = new Queue<Protobuf_TcpP2P_FilePush>();

            Thread thread = new Thread(receiver_StartRecvFileThread);
            thread.IsBackground = true;
            thread.Start(TaskID);
            App.log.Debug("[文件接收者]：准备接收线程");
        }

        /// <summary>
        /// 接收任务线程
        /// </summary>
        /// <param name="obj"></param>
        void receiver_StartRecvFileThread(object obj)
        {
            int TaskID = (int)obj;
            FilePushTask task = null;
            if (!DictTaskID2Task_RecvDo.ContainsKey(TaskID))
                return;

            task = DictTaskID2Task_RecvDo[TaskID];

            string fullpath = System.Environment.CurrentDirectory + "//" + task.FileName;
            FileInfo myFile = new FileInfo(fullpath);
            FileStream fs = myFile.Create();
            //int WriteLenght = 0;
            //TODO 打开文件写入
            while (true)
            {
                Queue<Protobuf_TcpP2P_FilePush> dataqueue = DictTaskID2Task_RecvDataQueue[TaskID];
                bool bflag = false;
                while (dataqueue.Count > 0)
                {
                    Protobuf_TcpP2P_FilePush msg = dataqueue.Dequeue();
                    if (msg == null)
                        continue;

                    bflag = true;
                    byte[] bytedata = msg.FileData.ToArray();
                    if (msg.PackIndex - task.CurrIndex != 1)
                    {
                        App.log.Error($"[文件接收者]接收下标错误，msg.PackIndex->{msg.PackIndex} task.CurrIndex=>{task.CurrIndex}");
                    }
                    //写入文件
                    fs.Write(bytedata, 0, msg.PackLenght);
                    //WriteLenght += msg.PackLenght;
                    task.CurrIndex = msg.PackIndex;
                    //todo 写入片段
                    //更新进度

                    //App.log.Debug($"[文件接收者]收到文件片段，下标->{task.CurrIndex}");
                }

                if (bflag)
                {
                    //回执收到信息
                    SendFilePush_Resp(task.fromUID, TaskID, task.CurrIndex);
                }

                if (task.CurrIndex + 1 >= task.PackCount)
                {
                    //文件接收完毕
                    App.log.Debug($"[文件接收者]文件接收完毕!! task.CurrIndex->{task.CurrIndex} task.PackCount->{task.PackCount}");
                    //删除进行中字典
                    DictTaskID2Task_RecvDo.Remove(TaskID);
                    //删除进行中数据队列
                    DictTaskID2Task_RecvDataQueue.Remove(TaskID);
                    break;
                }

                Thread.Sleep(10);
            }
            //TODO 结束写入
            fs.Close();
        }
        #endregion
    }

    public class FilePushTask
    {
        public int TaskID;//传输任务ID
        public long fromUID;//发送者UID
        public long targetUID;
        public string Path;
        public long FileLenght;//文件长度
        public string ContentType;//ContentType
        public string FileName;//文件名
        public int PackSize;//每个包大小
        public long PackCount;//包数量
        public string FileHash;//文件hash

        public long CurrIndex;
        public long ConfirmIndex;
    }
}
