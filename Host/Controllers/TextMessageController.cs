using InstantMultiplayer;
using InstantMultiplayer.Communication.Match;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

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
