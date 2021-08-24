using InstantMultiplayer.Synchronization.Attributes;
using InstantMultiplayer.Synchronization.Extensions;
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

        private readonly HashSet<Type> _genericDeclaringTypeMemberBlacklist;

        private MonitorFactory() {
            _componentProviders = new Dictionary<Type, IComponentMonitorProvider>();
            _memberProviders = new IMemberMonitorProvider[0];

            var monitorContainer = MonitorHelper.GetAllProviders();
            foreach (var componentMonitor in monitorContainer._componentMonitorProviders)
                InternalComponentRegisterProvider(componentMonitor);
            InternalMemberRegisterProvider(monitorContainer._memberMonitorProviders);
            _genericDeclaringTypeMemberBlacklist = new Type[]
            {
                typeof(MonoBehaviour),
                typeof(Behaviour),
                typeof(Component),
                typeof(UnityEngine.Object)
            }.ToHashSet();
        }

        public static ComponentMonitor CreateComponentMonitor(int id, Component componentInstance)
        {
            return Instance.InternalCreateComponentMonitor(id, componentInstance);
        }

        public static AMemberMonitorBase CreateMemberMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return Instance.InternalCreateMemberMonitor(memberHolder, memberInfo);
        }

        public static AMemberMonitorBase CreateGenericMemberMonitor(object memberHolder, MemberInfo memberInfo)
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

        private AMemberMonitorBase InternalCreateMemberMonitor(object memberHolder, MemberInfo memberInfo)
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

        private AMemberMonitorBase InternalCreateGenericMemberMonitor(object memberHolder, MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return new GenericMemberMonitor(memberInfo.Name, memberInfo.GetValueTypeFromMemberInfo(), () => ((FieldInfo)memberInfo).GetValue(memberHolder),
                        (val) => ((FieldInfo)memberInfo).SetValue(memberHolder, val));
                case MemberTypes.Property:
                    return new GenericMemberMonitor(memberInfo.Name, memberInfo.GetValueTypeFromMemberInfo(), () => ((PropertyInfo)memberInfo).GetValue(memberHolder),
                        (val) => ((PropertyInfo)memberInfo).SetValue(memberHolder, val));
                default:
                    throw new ArgumentException("Only with members of MemberType Field or Property with exposed read and write can a MemberMonitor be generically created!");
            }
        }

        private bool FieldIncluded(FieldInfo fieldInfo)
        {
            var included = false;
            if (_genericDeclaringTypeMemberBlacklist.Contains(fieldInfo.DeclaringType))
                return false;
            if (fieldInfo.IsObsolete())
                return false;
            if (fieldInfo.IsPublic)
                included = true;
            foreach (var data in fieldInfo.CustomAttributes)
                if (data.AttributeType == typeof(SerializeField))
                    included = true;
                else if(data.AttributeType == typeof(ExcludeSync))
                        return false;
            return included;
        }

        private bool PropertyIncluded(PropertyInfo propertyInfo)
        {
            if (_genericDeclaringTypeMemberBlacklist.Contains(propertyInfo.DeclaringType))
                return false;
            if (propertyInfo.IsObsolete())
                return false;
            return propertyInfo.IsPublicGetSetProperty()
                && (!propertyInfo.CustomAttributes.Any(data => data.AttributeType == typeof(ExcludeSync)));
        }

        private AMemberMonitorBase[] GenericMembers(object componentInstance)
        {
            var members = new List<AMemberMonitorBase>();
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

        public IEnumerable<FieldInfo> MonitorableFieldInfos(object componentInstance)
        {
            var type = componentInstance.GetType();
            var fields = MonitorableFieldInfos(type);
            return fields;
        }

        public IEnumerable<FieldInfo> MonitorableFieldInfos(Type componentType)
        {
            var fields = componentType.GetRuntimeFields().Where(FieldIncluded);
            return fields;
        }

        public IEnumerable<PropertyInfo> MonitorablePropertyInfos(object componentInstance)
        {
            var type = componentInstance.GetType();
            var properties = MonitorablePropertyInfos(type);
            return properties;
        }

        public IEnumerable<PropertyInfo> MonitorablePropertyInfos(Type componentType)
        {
            var properties = componentType.GetRuntimeProperties().Where(PropertyIncluded);
            return properties;
        }

        public IEnumerable<MemberInfo> MonitorableMemberInfos(object componentInstance)
        {
            var type = componentInstance.GetType();
            var members = MonitorableMemberInfos(type);
            return members;
        }

        public IEnumerable<MemberInfo> MonitorableMemberInfos(Type componentType)
        {
            var fields = componentType.GetRuntimeFields().Where(FieldIncluded);
            var properties = componentType.GetRuntimeProperties().Where(PropertyIncluded);
            return (fields as IEnumerable<MemberInfo>)?.Concat(properties) ?? Enumerable.Empty<MemberInfo>();
        }

        internal bool TryGetProviderMemberMonitors(Component componentInstance, out IEnumerable<AMemberMonitorBase> memberMonitors)
        {
            memberMonitors = _componentProviders.TryGetValue(componentInstance.GetType(), out var provider) ?
                provider.MonitoredMembers(componentInstance).ToArray() : null;
            return memberMonitors != null;
        }
    }
}
