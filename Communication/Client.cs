﻿using Communication;
using InstantMultiplayer.Communication.Match;
using InstantMultiplayer.Communication.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
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

        public EventHandler<ConnectionMessage> OnIdentified;

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
            tcpClient = new TcpClient(usedIp, usedPort);
            connected = true;

            var stream = tcpClient.GetStream();
            writer = new BinaryWriter(stream);
        }

        public int Poll()
        {
            int count = 0;
            while (tcpClient.Available != 0)
            {
                count++;
                var networkStream = tcpClient.GetStream();
                SendMessage(new MessageMatchLogin());

                if (networkStream.DataAvailable)
                {
                    var data = _binarySerializer.Deserialize(networkStream);
                    if(data is ConnectionMessage connectionMessage)
                    {
                        localId = connectionMessage.LocalId;
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
