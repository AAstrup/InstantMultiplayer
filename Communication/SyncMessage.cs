using InstantMultiplayer.UnityIntegration;
using System;
using System.Collections.Generic;

namespace InstantMultiplayer.Communication
{
    [Serializable]
    public class SyncMessage: IMessage
    {
        public List<DeltaContainer> Deltas;
    }
}
