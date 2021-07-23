using InstantMultiplayer.Synchronization.Delta;
using System;

namespace InstantMultiplayer.UnityIntegration
{
    [Serializable]
    public sealed class DeltaContainer
    {
        public int SynchronizerId;
        public int ClientFilter;
        public DeltaComponent[] Components;
    }
}
