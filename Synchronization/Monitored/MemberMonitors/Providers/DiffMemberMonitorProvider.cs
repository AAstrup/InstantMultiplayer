using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.DiffMemberMonitors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class DiffMemberMonitorProvider : IMemberMonitorProvider
    {
        private readonly Dictionary<Type, Type> _monitors;

        public int Precedence => int.MaxValue;

        public DiffMemberMonitorProvider()
        {
            _monitors = new Dictionary<Type, Type>
            {
                {typeof(Vector3), typeof(Vector3DiffMemberMonitor)}
            };
        }

        public AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            var memberValueType = memberInfo.GetValueTypeFromMemberInfo();
            if (!_monitors.TryGetValue(memberValueType, out var monitorType))
                throw new ArgumentException($"Failed to find a DiffMemberMonitor for {memberHolder} with type {memberValueType}");
            return (AMemberMonitorBase)Activator.CreateInstance(monitorType, memberInfo.Name, memberHolder, memberInfo);
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return _monitors.ContainsKey(memberInfo.GetValueTypeFromMemberInfo());
        }
    }
}
