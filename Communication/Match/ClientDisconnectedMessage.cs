using InstantMultiplayer.Communication;
using System;

namespace Communication.Match
{
    [Serializable]
    public class ClientDisconnectedMessage: IMessage
    {
        public int ClientId;
    }
}
