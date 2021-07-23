using InstantMultiplayer.Communication;
using InstantMultiplayer.Communication.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class TestCommunicationClient : IClient
    {
        private BinarySerializer _serializer;
        private Queue<byte[]> _incomingMessageQueue;
        private Queue<byte[]> _outgoingMessageQueue;

        public TestCommunicationClient()
        {
            _serializer = new BinarySerializer();
            _incomingMessageQueue = new Queue<byte[]>();
            _outgoingMessageQueue = new Queue<byte[]>();
        }

        public void SendMessage(IMessage message)
        {
            var ser = _serializer.Serialize(message);
            _outgoingMessageQueue.Enqueue(ser);
        }

        public bool TryRecieveMessage(out IMessage message)
        {
            if (_incomingMessageQueue.Count != 0)
            {
                var ser = _incomingMessageQueue.Dequeue();
                message = (IMessage)_serializer.Deserialize(ser);
                return true;
            }
            message = null;
            return false;
        }

        public int Flush()
        {
            var c = _outgoingMessageQueue.Count;
            while (_outgoingMessageQueue.Count != 0)
            {
                var ser = _outgoingMessageQueue.Dequeue();
                _incomingMessageQueue.Enqueue(ser);
            }
            return c;
        }
    }
}
