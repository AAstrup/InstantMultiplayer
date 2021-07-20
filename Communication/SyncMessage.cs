using InstantMultiplayer.UnityIntegration;
using System.Collections.Generic;

namespace InstantMultiplayer.Communication
{
    public class SyncMessage: IMessage
    {
        public List<DeltaContainer> Deltas;
    }
}
