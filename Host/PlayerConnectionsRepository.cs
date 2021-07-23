using InstantMultiplayer.Communication.Match;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstantMultiplayer
{
    public class PlayerConnectionsRepository
    {
        private Dictionary<int, TcpClient> playeridToConnection;
        private int tempGetNextId;

        public PlayerConnectionsRepository()
        {
            playeridToConnection = new Dictionary<int, TcpClient>();
        }

        public void AddPlayer(int id, TcpClient client)
        {
            // PlayerId's er not used - always treat login as new players
            var tempId = TEMPGetNextId();
            if (playeridToConnection.ContainsKey(tempId))
                playeridToConnection[tempId] = client;
            else
                playeridToConnection.Add(tempId, client);
        }

        private int TEMPGetNextId()
        {
            tempGetNextId += 1;
            return tempGetNextId;
        }

        public void SendToAllClients(object serilizableObjectToSend)
        {
            var players = playeridToConnection.Values.ToList();
            foreach (var player in players)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var writer = new BinaryWriter(player.GetStream());
                byte[] bytes;
                using (MemoryStream memory = new MemoryStream())
                {
                    formatter.Serialize(memory, serilizableObjectToSend);
                    bytes = memory.ToArray();
                }

                writer.Write(bytes);
            }
        }
    }
}
