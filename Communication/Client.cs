using System;
using System.Collections.Generic;

namespace InstantMultiplayer.Communication
{
    public class Client
    {
        public Client()
        {
            IncomingMessageQueue = new Queue<IMessage>();
        }

        public Queue<IMessage> IncomingMessageQueue;
        public void SendMessage(IMessage message)
        {
            //Det her er meget primitivt i know ;)
            //Blot for at have et fuldendt setup med SyncClient fra UnityIntegration
        }
    }
}
