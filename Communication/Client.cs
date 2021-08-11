using Communication;
using Communication.Match;
using GuerrillaNtp;
using InstantMultiplayer.Communication.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace InstantMultiplayer.Communication
{
    public class Client
    {
        private readonly BinarySerializer _binarySerializer;
        private BinaryWriter writer;
        private TcpClient tcpClient;

        // For Denbugging
        public bool connected;
        public bool identified;
        public int localId;
        public Queue<IMessage> incomingMessageQueue;
        public TimeSpan NTPOffset;
        public DateTime InitialSyncedTimestamp;

        public EventHandler<GreetMessage> OnIdentified;

        public float SyncTime => (float)(DateTime.Now + NTPOffset - InitialSyncedTimestamp).TotalSeconds;

        public Client(string ip, int port)
        {
            incomingMessageQueue = new Queue<IMessage>();
            _binarySerializer = new BinarySerializer();
            Connect(ip, port);
        }

        public void SendMessage(IMessage message)
        {
            if (!connected) throw new Exception("You are not connected!");
            writer.Write(_binarySerializer.Serialize(message));
        }

        private void Connect(string usedIp, int usedPort)
        {
            using (var ntp = new NtpClient(Dns.GetHostAddresses("pool.ntp.org")[0]))
                NTPOffset = ntp.GetCorrectionOffset();

            tcpClient = new TcpClient(usedIp, usedPort);
            connected = true;

            var stream = tcpClient.GetStream();
            writer = new BinaryWriter(stream);
            SendMessage(new MessageMatchLogin());
        }

        public int Poll()
        {
            int count = 0;
            while (tcpClient.Available != 0)
            {
                count++;
                var networkStream = tcpClient.GetStream();

                if (networkStream.DataAvailable)
                {
                    var data = _binarySerializer.Deserialize(networkStream);
                    if(data is GreetMessage connectionMessage)
                    {
                        localId = connectionMessage.LocalId;
                        InitialSyncedTimestamp = connectionMessage.InitialSyncTimestamp;
                        identified = true;
                        SendMessage(new GetHistoryMessage());
                        OnIdentified.Invoke(this, connectionMessage);
                        continue;
                    }
                    incomingMessageQueue.Enqueue((IMessage)data);
                }
            }
            return count;
        }
    }
}
