using System.Collections.Concurrent;

namespace InstantMultiplayer.Communication
{
    public class Client: IClient
    {
        private ConcurrentQueue<IMessage> _incomingMessageQueue;

        public Client()
        {
            _incomingMessageQueue = new ConcurrentQueue<IMessage>();
        }

        public bool TryRecieveMessage(out IMessage message)
        {
            return _incomingMessageQueue.TryDequeue(out message);
        }

        public void SendMessage(IMessage message)
        {
            //Det her er meget primitivt i know ;)
            //Blot for at have et fuldendt setup med SyncClient fra UnityIntegration
        }
    }
}
