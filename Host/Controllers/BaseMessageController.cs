using System;
using System.Net.Sockets;

namespace Host.Controllers
{
    public abstract class BaseMessageController<T> : IMessageController where T : class 
    {
        public Type GetHandlerType()
        {
            return typeof(T);
        }

        public abstract void HandleMessage(T message, TcpClient tcpClient);

        private void Handler(object message, TcpClient client)
        {
            HandleMessage((T)message, client);
        }

        public Action<object, TcpClient> GetMessageHandler()
        {
            return Handler;
        }
    }
}