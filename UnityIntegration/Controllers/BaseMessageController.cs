using InstantMultiplayer.Communication;
using System;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public abstract class BaseMessageController<T> : IMessageController
    {
        public Type GetMessageType() => typeof(T);
        public abstract void HandleMessage(T message);

        public void HandleMessage(object message)
        {
            HandleMessage((T)message);
        }

        public abstract bool TryGetMessage(out IMessage message);
    }
}