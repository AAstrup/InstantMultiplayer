using Synchronization.HashCodes.Implementations;
using System;
using System.Collections.Generic;

namespace Synchronization.HashCodes
{
    public class IdFactory
    {
        public static IdFactory Instance => _instance ?? (_instance = new IdFactory());
        private static IdFactory _instance;

        private readonly Dictionary<Type, IIdProvider> _providers;

        private IdFactory()
        {
            _providers = new Dictionary<Type, IIdProvider>();
            Register(new MeshIdProvider());
        }

        public void Register(IIdProvider hashCodeProvider)
        {
            _providers.Add(hashCodeProvider.Type, hashCodeProvider);
        }

        public bool TryGetId(object obj, out int hashCode)
        {
            if (_providers.TryGetValue(obj.GetType(), out var prov))
            {
                hashCode = prov.GetHashCode(obj);
                return true;
            }
            hashCode = 0;
            return false;
        }

        public int GetId(object obj)
        {
            if (_providers.TryGetValue(obj.GetType(), out var prov))
            {
                return prov.GetHashCode(obj);
            }
            return obj.GetHashCode();
        }

    }
}
