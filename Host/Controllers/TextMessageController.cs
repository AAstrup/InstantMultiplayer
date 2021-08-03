using Communication.Match;
using InstantMultiplayer;
using System;
using System.Net.Sockets;

namespace Host.Controllers
{
    public class TextMessageController : BaseMessageController<MessageText>
    {
        private PlayerConnectionsRepository _playerConnectionsRepository;

        public TextMessageController(PlayerConnectionsRepository playerConnectionsRepository)
        {
            _playerConnectionsRepository = playerConnectionsRepository ?? throw new NullReferenceException(nameof(playerConnectionsRepository));
        }

        public override void HandleMessage(MessageText message, TcpClient tcpClient)
        {
            _playerConnectionsRepository.SendToAllClients(message);
        }
    }
}
