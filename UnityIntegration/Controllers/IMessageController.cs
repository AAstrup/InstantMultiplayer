using InstantMultiplayer.Communication;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public interface IMessageController
    {
        bool TryGetMessage(out IMessage message);
        void HandleMessage(object message);
    }
}