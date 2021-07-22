using InstantMultiplayer;
using InstantMultiplayer.Communication.Match;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

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
