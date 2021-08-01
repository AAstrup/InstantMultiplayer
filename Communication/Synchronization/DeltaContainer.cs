using InstantMultiplayer.Synchronization.Delta;
using System;

namespace Communication.Synchronization
{
    [Serializable]
    public sealed class DeltaContainer
    {
        public int SynchronizerId;
        public int ClientFilter;
        public DeltaComponent[] Components;
    }
}
