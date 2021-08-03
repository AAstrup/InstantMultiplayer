using InstantMultiplayer.Communication;
using System;

namespace Communication.Synchronization.Events
{
    [Serializable]
    public abstract class ASyncEventMessage: IMessage
    {
        public int ClientFilter;
    }
}
