using Communication.Match;
using InstantMultiplayer.Communication;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class ClientConnectedMessageController : BaseMessageController<ClientConnectedMessage>
    {
        public static ClientConnectedMessageController Instance => _instance ?? (_instance = new ClientConnectedMessageController());
        private static ClientConnectedMessageController _instance;

        public override void HandleMessage(ClientConnectedMessage syncMessage)
        {
            
        }

        public override bool TryGetMessage(out IMessage message)
        {
            message = null;
            return false;
        }
    }
}
