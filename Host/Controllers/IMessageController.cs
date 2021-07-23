using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Host.Controllers
{
    public interface IMessageController
    {
        Action<object, TcpClient> GetMessageHandler();
        Type GetHandlerType();
    }
}