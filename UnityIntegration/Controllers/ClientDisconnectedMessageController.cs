using Communication.Match;
using InstantMultiplayer.Communication;
using System.Linq;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class ClientDisconnectedMessageController : BaseMessageController<ClientDisconnectedMessage>
    {
        public static ClientDisconnectedMessageController Instance => _instance ?? (_instance = new ClientDisconnectedMessageController());
        private static ClientDisconnectedMessageController _instance;

        public override void HandleMessage(ClientDisconnectedMessage syncMessage)
        {
            var ownedSynchronizers = SynchronizeStore.Instance.Synchronizers.Where(s => s.OwnerId == syncMessage.ClientId).ToList();
            foreach(var ownedSynchronizer in ownedSynchronizers)
            {
                SynchronizeStore.Instance.ExhaustId(ownedSynchronizer.SynchronizerId);
                UnityEngine.Object.Destroy(ownedSynchronizer.gameObject);
            }
        }

        public override bool TryGetMessage(out IMessage message)
        {
            message = null;
            return false;
        }
    }
}
