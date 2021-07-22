using InstantMultiplayer.Synchronization.Delta;
using System;
using System.Collections.Generic;

namespace InstantMultiplayer.UnityIntegration
{
    [Serializable]
    public sealed class DeltaContainer
    {
        public int SynchronizerId;
        public int Filter;
        public DeltaComponent[] Components;
    }
}
