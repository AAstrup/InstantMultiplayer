using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored
{
    public class MonitorFactory
    {
        public static MonitorFactory Instance => _instance ?? (_instance = new MonitorFactory());

        private static MonitorFactory _instance;
        private readonly Dictionary<Type, IComponentMonitorProvider> _componentProviders;
        private IMemberMonitorProvider[] _memberProviders;

        private MonitorFactory() {
            _componentProviders = new Dictionary<Type, IComponentMonitorProvider>();
            _memberProviders = new IMemberMonitorProvider[0];
            foreach (var componentMonitor in MonitorDefaults.ComponentMonitors())
                InternalComponentRegisterProvider(componentMonitor);
            InternalMemberRegisterProvider(MonitorDefaults.MemberMonitors());
        }

        public static ComponentMonitor CreateComponentMonitor(int id, Component componentInstance)
        {
            return Instance.InternalCreateComponentMonitor(id, componentInstance);
        }

        public static MemberMonitor CreateMemberMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return Instance.InternalCreateMemberMonitor(memberHolder, memberInfo);
        }

        public static MemberMonitor CreateGenericMemberMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return Instance.InternalCreateGenericMemberMonitor(memberHolder, memberInfo);
        }

        public static void RegisterComponentProvider(IComponentMonitorProvider monitorProvider)
        {
            Instance.InternalComponentRegisterProvider(monitorProvider);
        }

        public static void RegisterMemberProvider(IMemberMonitorProvider memberProvider)
        {
            Instance.InternalMemberRegisterProvider(memberProvider);
        }

        private ComponentMonitor InternalCreateComponentMonitor(int id, Component componentInstance)
        {
            var fields = _componentProviders.TryGetValue(componentInstance.GetType(), out var provider) ?
                provider.MonitoredMembers(componentInstance).ToArray() :
                GenericMembers(componentInstance);
            return new ComponentMonitor(id, componentInstance, fields);
        }

        private MemberMonitor InternalCreateMemberMonitor(object memberHolder, MemberInfo memberInfo)
        {
            //var value = GetValueFromMemberInfo(memberHolder, memberInfo);
            foreach (var provider in _memberProviders)
                if (provider.IsApplicable(memberHolder, memberInfo))
                    return provider.GetMonitor(memberHolder, memberInfo);
            return CreateGenericMemberMonitor(memberHolder, memberInfo);
        }

        private void InternalComponentRegisterProvider(IComponentMonitorProvider monitorProvider)
        {
            foreach (var type in monitorProvider.ComponentTypes())
                if(!_componentProviders.ContainsKey(type))
                    _componentProviders.Add(type, monitorProvider);
        }

        private void InternalMemberRegisterProvider(IMemberMonitorProvider memberProvider)
        {
            _memberProviders = _memberProviders.Append(memberProvider).OrderBy(p => p.Precedence).ToArray();
        }

        private void InternalMemberRegisterProvider(IEnumerable<IMemberMonitorProvider> memberProviders)
        {
            _memberProviders = _memberProviders.Concat(memberProviders).OrderBy(p => p.Precedence).ToArray();
        }

        private MemberMonitor InternalCreateGenericMemberMonitor(object memberHolder, MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return new MemberMonitor(() => ((FieldInfo)memberInfo).GetValue(memberHolder),
                        (val) => ((FieldInfo)memberInfo).SetValue(memberHolder, val));
                case MemberTypes.Property:
                    return new MemberMonitor(() => ((PropertyInfo)memberInfo).GetValue(memberHolder),
                        (val) => ((PropertyInfo)memberInfo).SetValue(memberHolder, val));
                default:
                    throw new ArgumentException("Only with members of MemberType Field or Property with exposed read and write can a MemberMonitor be generically created!");
            }
        }

        private bool FieldIncluded(FieldInfo fieldInfo)
        {
            return fieldInfo.IsPublic || fieldInfo.CustomAttributes.Any(
                data => data.AttributeType == typeof(SerializeField));
        }

        private bool PropertyIncluded(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetSetMethod(true).IsPublic;
        }

        private MemberMonitor[] GenericMembers(object componentInstance)
        {
            var members = new List<MemberMonitor>();
            var type = componentInstance.GetType();
            var fields = type.GetRuntimeFields().Where(FieldIncluded);
            members.AddRange(fields.Select(f =>
                CreateMemberMonitor(componentInstance, f)
            ));
            var props = type.GetRuntimeProperties().Where(PropertyIncluded);
            members.AddRange(props.Select(p =>
                CreateMemberMonitor(componentInstance, p)
            ));
            return members.ToArray();
        }
    }
}
