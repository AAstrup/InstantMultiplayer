using System;

namespace InstantMultiplayer.Synchronization.Delta
{
    [Serializable]
    public sealed class DeltaMember
    {
        public int Index;
        public object Value; // Must be serializable
        public int TimeStamp;
    }
}
