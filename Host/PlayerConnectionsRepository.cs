using Communication;
using Communication.Match;
using InstantMultiplayer.Communication.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace InstantMultiplayer
{
    public class PlayerConnectionsRepository
    {
        private readonly Dictionary<int, TcpClient> _playeridToConnection;
        private readonly Dictionary<TcpClient, int> _connectionToId;
        private int tempGetNextId;

        public PlayerConnectionsRepository()
        {
            _playeridToConnection = new Dictionary<int, TcpClient>();
            _connectionToId = new Dictionary<TcpClient, int>();
        }

        public void AddPlayer(int id, TcpClient client)
        {
            // PlayerId's er not used - always treat login as new players
            var tempId = TEMPGetNextId();
            Console.WriteLine($"New ID {tempId} gotten for client at IP " + client.Client.RemoteEndPoint);
            if (_playeridToConnection.ContainsKey(tempId))
            {
                _playeridToConnection[tempId] = client;
                _connectionToId[client] = tempId;
            }
            else
            {
                _playeridToConnection.Add(tempId, client);
                _connectionToId.Add(client, tempId);
            }
            SendToClient(client, new GreetMessage { LocalId = tempId });
            var connectedMessage = new ClientConnectedMessage
            {
                ClientId = tempId
            };
            foreach (var recipient in GetClients())
            {
                SendToClient(recipient, connectedMessage);
            }
        }

        private int TEMPGetNextId()
        {
            tempGetNextId += 1;
            return tempGetNextId;
        }

        public void SendToAllClients(object serilizableObjectToSend)
        {
            var players = _playeridToConnection.Values.ToList();
            foreach (var player in players)
                SendToClient(player, serilizableObjectToSend);
        }

        public void SendToClient(TcpClient player, object serilizableObjectToSend)
        {
            try
            {
                var writer = new BinaryWriter(player.GetStream());
                var bytes = new BinarySerializer().Serialize(serilizableObjectToSend);
                writer.Write(bytes);
            }
            catch(Exception)
            {
                RemoveClient(player);
            }
        }

        private void RemoveClient(TcpClient player)
        {
            var id = _connectionToId[player];
            _connectionToId.Remove(player);
            _playeridToConnection.Remove(id);
            SendToAllClients(new ClientDisconnectedMessage
            {
                ClientId = id
            });
        }

        public bool TryGetClient(int playerId, out TcpClient player)
        {
            return _playeridToConnection.TryGetValue(playerId, out player);
        }

        public IEnumerable<TcpClient> GetClients()
        {
            return _playeridToConnection.Values;
        }
    }
}
