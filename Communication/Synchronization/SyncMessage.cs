using InstantMultiplayer.Communication;
using System;
using System.Collections.Generic;

namespace Communication.Synchronization
{
    [Serializable]
    public class SyncMessage: IMessage
    {
        public List<DeltaContainer> Deltas;
    }
}
