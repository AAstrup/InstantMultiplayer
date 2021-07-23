using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace InstantMultiplayer.Communication
{
    public class Client
    {
        private BinaryReader reader;
        private BinaryWriter writer;
        private IFormatter formatter;
        private TcpClient tcpClient;
        // For Denbugging
        public bool connected;
        public Queue<IMessage> incomingMessageQueue;

        public Client(string ip, int port)
        {
            incomingMessageQueue = new Queue<IMessage>();
            Connect(ip, port);
        }

        public void SendMessage(IMessage message)
        {
            if (!connected) throw new Exception("You are not connected!");

            IFormatter formatter = new BinaryFormatter();
            byte[] bytes;
            using (MemoryStream memory = new MemoryStream())
            {
                formatter.Serialize(memory, message);
                bytes = memory.ToArray();
            }

            writer.Write(bytes);
        }

        private void Connect(string usedIp, int usedPort)
        {
            tcpClient = new TcpClient(usedIp, usedPort);
            connected = true;

            var stream = tcpClient.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            formatter = new BinaryFormatter();
        }

        public void Poll()
        {
            while (true)
            {
                if (tcpClient.Available == 0) break;
                NetworkStream networkStream = tcpClient.GetStream();

                if (networkStream.DataAvailable)
                {
                    var data = formatter.Deserialize(networkStream);
                    incomingMessageQueue.Enqueue((IMessage)data);
                }
            }
        }
    }
}
