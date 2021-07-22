using SynchronizationFR.Monitored.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SynchronizationFR.Monitored
{
    public class MonitorFactory
    {
        private static MonitorFactory _instance;
        public static MonitorFactory Instance => _instance ?? (_instance = new MonitorFactory());

        private MonitorFactory() {
            _providers = new Dictionary<Type, IMonitorProvider>();
            RegisterProvider(new SyncedTransform());
        }

        public static MonitoredComponent CreateComponentMonitor(Component componentInstance)
        {
            return Instance.InternalCreateComponentMonitor(componentInstance);
        }

        private MonitoredComponent InternalCreateComponentMonitor(Component componentInstance)
        {
            var fields = _providers.TryGetValue(componentInstance.GetType(), out var provider) ?
                provider.MonitoredMembers(componentInstance).ToArray() :
                GenericMembers(componentInstance);
            return new MonitoredComponent(componentInstance.name.GetHashCode(), fields);
        }

        public void RegisterProvider(IMonitorProvider monitorProvider)
        {
            foreach (var type in monitorProvider.ComponentTypes())
                if(!_providers.ContainsKey(type))
                    _providers.Add(type, monitorProvider);
        }

        private Dictionary<Type, IMonitorProvider> _providers;

        private bool FieldIncluded(FieldInfo fieldInfo)
        {
            return fieldInfo.IsPublic || fieldInfo.CustomAttributes.Any(
                data => data.AttributeType == typeof(SerializeField));
        }

        private bool PropertyIncluded(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetSetMethod(true).IsPublic;
        }

        private MonitoredMember[] GenericMembers(object componentInstance)
        {
            var members = new List<MonitoredMember>();
            var type = componentInstance.GetType();
            var fields = type.GetRuntimeFields().Where(FieldIncluded);
            members.AddRange(fields.Select(f =>
                new MonitoredMember(() => f.GetValue(componentInstance), (v) => f.SetValue(componentInstance, v))
            ));
            var props = type.GetRuntimeProperties().Where(PropertyIncluded);
            members.AddRange(props.Select(p =>
                new MonitoredMember(() => p.GetValue(componentInstance), (v) => p.SetValue(componentInstance, v))
            ));
            return members.ToArray();
        }
    }
}
