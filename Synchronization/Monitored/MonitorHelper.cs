using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored
{
    internal static class MonitorHelper
    {
        internal static MonitorProviderContainer GetAllProviders()
        {
            var componentMonitorProviderType = typeof(IComponentMonitorProvider);
            var memberMonitorProviderType = typeof(IMemberMonitorProvider);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass);

            var componentMonitors = new List<IComponentMonitorProvider>();
            var memberMonitorProvider = new List<IMemberMonitorProvider>();
            foreach (var type in types)
            {
                if (componentMonitorProviderType.IsAssignableFrom(type))
                {
                    try
                    {
                        var instance = Activator.CreateInstance(type) as IComponentMonitorProvider;
                        if (instance == null)
                            continue;
                        componentMonitors.Add(instance);
                    }
                    catch (Exception)
                    {
                        Debug.LogWarning($"Failed to create instance of IComponentMonitorProvider implementor {type.FullName}. Ensure that it has a parameterless constructor.");
                    }
                }
                else if (memberMonitorProviderType.IsAssignableFrom(type))
                {
                    try
                    {
                        var instance = Activator.CreateInstance(type) as IMemberMonitorProvider;
                        if (instance == null)
                            continue;
                        memberMonitorProvider.Add(instance);
                    }
                    catch (Exception)
                    {
                        Debug.LogWarning($"Failed to create instance of IMemberMonitorProvider implementor {type.FullName}. Ensure that it has a parameterless constructor.");
                    }
                }
            }
            return new MonitorProviderContainer(componentMonitors, memberMonitorProvider);
        }
    }

    internal class MonitorProviderContainer
    {
        internal readonly IEnumerable<IComponentMonitorProvider> _componentMonitorProviders;
        internal readonly IEnumerable<IMemberMonitorProvider> _memberMonitorProviders;

        public MonitorProviderContainer(IEnumerable<IComponentMonitorProvider> componentMonitorProviders, IEnumerable<IMemberMonitorProvider> memberMonitorBases)
        {
            _componentMonitorProviders = componentMonitorProviders ?? throw new ArgumentNullException(nameof(componentMonitorProviders));
            _memberMonitorProviders = memberMonitorBases ?? throw new ArgumentNullException(nameof(memberMonitorBases));
        }
    }
}
