using InstantMultiplayer.Communication;
using System;

namespace Communication.Synchronization
{
    [Serializable]
    public abstract class ASyncEventMessage: IMessage
    {
        public int ClientFilter;
    }
}
