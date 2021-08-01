using InstantMultiplayer;
using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization.Filtering;
using System.Net.Sockets;

namespace Host.Controllers
{
    public class SyncMessageController : BaseMessageController<SyncMessage>
    {
        private PlayerConnectionsRepository _playerConnectionsRepository;



        public SyncMessageController(PlayerConnectionsRepository playerConnectionsRepository)
        {
            _playerConnectionsRepository = playerConnectionsRepository;
        }

        public override void HandleMessage(SyncMessage message, TcpClient tcpClient)
        {
            foreach (var delta in message.Deltas)
                for (int i = 0; i < 32; i++)
                    if (ClientFilterHelper.ClientIncluded(delta.ClientFilter, i))
                        if (_playerConnectionsRepository.TryGetClient(i, out var client))
                            if (client != tcpClient)
                                _playerConnectionsRepository.SendToClient(client, message);

            //For now we do this instead
            //foreach (var delta in message.Deltas)
            //    foreach(var client in _playerConnectionsRepository.GetClients())
            //        if (client != tcpClient)
            //            _playerConnectionsRepository.SendToClient(client, message);
        }
    }
}
