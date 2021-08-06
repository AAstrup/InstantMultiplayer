using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification
{
    public static class IdHelper
    {
        public static IEnumerable<IIdProvider> GetAllProviders()
        {
            var idProviderType = typeof(IIdProvider);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && idProviderType.IsAssignableFrom(t));
            var providers = new List<IIdProvider>();
            foreach(var type in types)
            {
                try
                {
                    var instance = Activator.CreateInstance(type) as IIdProvider;
                    if (instance == null)
                        continue;
                    providers.Add(instance);
                }
                catch(Exception)
                {
                    Debug.LogWarning($"Failed to create instance of IIdProvider implementor {type.FullName}. Ensure that is has a parameterless constructor.");
                }
            }
            return providers;
        }
        
    }
}
