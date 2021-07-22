using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMultiplayer.Synchronization.Delta
{
    [Serializable]
    public sealed class DeltaComponent
    {
        public int Id;
        public DeltaMember[] Members;
    }
}
