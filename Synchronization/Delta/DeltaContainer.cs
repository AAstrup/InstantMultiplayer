using System.Collections.Generic;

namespace InstantMultiplayer.Synchronization.Delta
{
    public sealed class DeltaContainer
    {
        public int SynchronizerId;
        public List<DeltaComponent> Components;
    }
}
