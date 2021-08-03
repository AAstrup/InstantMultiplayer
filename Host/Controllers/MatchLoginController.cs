using Communication.Match;
using InstantMultiplayer;
using System;
using System.Net.Sockets;

namespace Host.Controllers
{
    public class MatchLoginController : BaseMessageController<MessageMatchLogin>
    {
        private PlayerConnectionsRepository _playerConnectionsRepository;

        public MatchLoginController(PlayerConnectionsRepository playerConnectionsRepository)
        {
            _playerConnectionsRepository = playerConnectionsRepository ?? throw new NullReferenceException(nameof(playerConnectionsRepository));
        }

        public override void HandleMessage(MessageMatchLogin message, TcpClient tcpClient)
        {
            _playerConnectionsRepository.AddPlayer(message.id, tcpClient);
        }
    }
}
