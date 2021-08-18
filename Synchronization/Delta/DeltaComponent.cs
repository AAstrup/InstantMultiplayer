using System;

namespace InstantMultiplayer.Synchronization.Delta
{
    [Serializable]
    public sealed class DeltaComponent
    {
        public int Id;
        public int TypeId;
        public DeltaMember[] Members;
    }
}
