using Communication;
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
        private int tempGetNextId;

        public PlayerConnectionsRepository()
        {
            _playeridToConnection = new Dictionary<int, TcpClient>();
        }

        public void AddPlayer(int id, TcpClient client)
        {
            // PlayerId's er not used - always treat login as new players
            var tempId = TEMPGetNextId();
            Console.WriteLine($"New ID {tempId} gotten for client at IP " + client.Client.RemoteEndPoint);
            if (_playeridToConnection.ContainsKey(tempId))
                _playeridToConnection[tempId] = client;
            else
                _playeridToConnection.Add(tempId, client);
            SendToClient(client, new ConnectionMessage { LocalId = tempId });
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
            var writer = new BinaryWriter(player.GetStream());
            var bytes = new BinarySerializer().Serialize(serilizableObjectToSend);
            writer.Write(bytes);
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
