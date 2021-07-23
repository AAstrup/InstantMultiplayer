using InstantMultiplayer.Communication;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public interface IMessageController
    {
        public abstract bool TryGetMessage(out IMessage message);
        public abstract void HandleMessage(object message);
    }
}