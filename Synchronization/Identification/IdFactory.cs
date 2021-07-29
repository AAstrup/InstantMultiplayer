using InstantMultiplayer.Synchronization.Identification.Implementations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification
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
            Register(new ArrayIdProvider());
            Register(new MaterialIdProvider());
            Register(new Texture2DIdProvider());
            Register(new TextureIdProvider());
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
            return GetId(obj, obj.GetType());
        }

        public int GetId(object obj, Type type)
        {
            if (_providers.TryGetValue(type, out var prov))
            {
                return prov.GetHashCode(obj);
            }
            Debug.LogWarning("Failed to get id specific for " + type);
            return obj.GetHashCode();
        }

        public bool RegisteredType(Type type)
        {
            return _providers.ContainsKey(type);
        }

    }
}
