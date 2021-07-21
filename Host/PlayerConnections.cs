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
    public class PlayerConnections
    {
        private Dictionary<int, TcpClient> playeridToConnection;
        private int tempGetNextId;

        public PlayerConnections()
        {
            playeridToConnection = new Dictionary<int, TcpClient>();
        }

        public void AddPlayer(int id, TcpClient client)
        {
            if (playeridToConnection.ContainsKey(id))
                playeridToConnection[id] = client;
            else
                playeridToConnection.Add(id, client);
        }

        internal int TEMPGetNextId()
        {
            tempGetNextId += 1;
            return tempGetNextId;
        }

        internal List<TcpClient> TEMPGetAllPlayers()
        {
            return playeridToConnection.Values.ToList();
        }
    }
}
