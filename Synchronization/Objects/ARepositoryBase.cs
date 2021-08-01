using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstantMultiplayer.Synchronization.Objects
{
    public abstract class ARepositoryBase
    {
        public abstract bool TryGetObject(int id, Type type, out UnityEngine.Object obj);
    }
}
