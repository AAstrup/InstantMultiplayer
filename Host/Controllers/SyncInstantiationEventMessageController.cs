using Communication;
using Communication.Synchronization;
using Host.Controllers;
using InstantMultiplayer.Synchronization.Filtering;
using System.Net.Sockets;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class SyncInstantiationEventMessageController : BaseMessageController<ASyncEventMessage>
    {
        private PlayerConnectionsRepository _playerConnectionsRepository;

        public SyncInstantiationEventMessageController(PlayerConnectionsRepository playerConnectionsRepository)
        {
            _playerConnectionsRepository = playerConnectionsRepository;
        }

        public override void HandleMessage(ASyncEventMessage message, TcpClient tcpClient)
        {
            for (int i = 0; i < 32; i++)
                if (ClientFilterHelper.ClientIncluded(message.ClientFilter, i))
                    if (_playerConnectionsRepository.TryGetClient(i, out var client))
                        if (client != tcpClient)
                            _playerConnectionsRepository.SendToClient(client, message);
        }
    }
}
