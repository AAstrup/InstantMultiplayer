using Communication.Synchronization.Events;
using Host.Controllers;
using InstantMultiplayer.Synchronization.Filtering;
using System.Net.Sockets;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class SyncDestroyEventMessageController : BaseMessageController<SyncDestroyEventMessage>
    {
        private PlayerConnectionsRepository _playerConnectionsRepository;

        public SyncDestroyEventMessageController(PlayerConnectionsRepository playerConnectionsRepository)
        {
            _playerConnectionsRepository = playerConnectionsRepository;
        }

        public override void HandleMessage(SyncDestroyEventMessage message, TcpClient tcpClient)
        {
            for (int i = 0; i < 32; i++)
                if (ClientFilterHelper.ClientIncluded(message.ClientFilter, i))
                    if (_playerConnectionsRepository.TryGetClient(i, out var client))
                        if (client != tcpClient)
                            _playerConnectionsRepository.SendToClient(client, message);
        }
    }
}
