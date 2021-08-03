using InstantMultiplayer.Communication;
using System;

namespace Communication.Match
{
    [Serializable]
    public class ClientConnectedMessage: IMessage
    {
        public int ClientId;
    }
}
