using Communication.Match;
using InstantMultiplayer;
using InstantMultiplayer.Communication;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Host.Controllers
{
    public class HistoryController : BaseMessageController<GetHistoryMessage>
    {
        private List<IMessage> _messages;
        private PlayerConnectionsRepository _playerConnectionsRepository;

        public HistoryController(PlayerConnectionsRepository playerConnectionsRepository, Events events)
        {
            events.messageClientIdRecieved += AddMessageToStack;
            _playerConnectionsRepository = playerConnectionsRepository;
            _messages = new List<IMessage>();
        }

        private void AddMessageToStack(object sender, KeyValuePair<IMessage, TcpClient> e)
        {
            _messages.Add(e.Key);
        }

        public override void HandleMessage(GetHistoryMessage historyMessage, TcpClient tcpClient)
        {
            foreach (var message in _messages)
            {
                _playerConnectionsRepository.SendToClient(tcpClient, message);
            }
        }
    }
}
