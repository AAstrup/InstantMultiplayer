using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstantMultiplayer.Synchronization.Objects
{
    public abstract class ARepository<T>: ARepositoryBase
    {
        protected Dictionary<string, Dictionary<int, T>> _map;

        public Dictionary<string, Dictionary<int, T>> MapCopy()
        {
            return _map.ToDictionary(p => p.Key, p => p.Value.ToDictionary(pp => pp.Key, pp => pp.Value));
        }
    }
}
