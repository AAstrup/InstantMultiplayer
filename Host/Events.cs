using InstantMultiplayer.Communication;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InstantMultiplayer
{
    public class Events
    {
        public EventHandler<KeyValuePair<IMessage, TcpClient>> messageClientIdRecieved;
    }
}
