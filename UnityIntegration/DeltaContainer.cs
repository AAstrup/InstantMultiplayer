using InstantMultiplayer.Synchronization.Delta;
using System.Collections.Generic;

namespace InstantMultiplayer.UnityIntegration
{
    public sealed class DeltaContainer
    {
        public int SynchronizerId;
        public DeltaComponent[] Components;
    }
}
