﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCore.Event
{
    public enum EEvent
    {
        // 添加你自己需要的事件类型
        UserLogin,
        UserJoin,
        UserLeave,
        OnChatMsg,

        //打洞流程
        TcpTunnelHelloResp,

        //连接管理
        OnSocketConnect,
        OnSocketDisconnect,
    }
}
