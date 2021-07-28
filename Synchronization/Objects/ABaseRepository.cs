using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Synchronization.Objects
{
    public abstract class ABaseRepository<T>
    {
        protected Dictionary<string, Dictionary<int, T>> _map;

        public Dictionary<string, Dictionary<int, T>> MapCopy()
        {
            return _map.ToDictionary(p => p.Key, p => p.Value.ToDictionary(pp => pp.Key, pp => pp.Value));
        }

        public abstract bool TryGetObject(int id, Type type, out UnityEngine.Object obj);
    }
}
