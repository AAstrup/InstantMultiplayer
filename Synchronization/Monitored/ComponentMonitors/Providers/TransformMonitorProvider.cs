using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class TransformMonitorProvider : IComponentMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            yield return typeof(TransformMonitorProvider);
        }

        public IEnumerable<MemberMonitor> MonitoredMembers(Component componentInstance)
        {
            var transform = (Transform)componentInstance;
            return new MemberMonitor[]
            {
                new MemberMonitor(() => transform.localPosition, (val) => transform.localPosition = (Vector3)val),
                new MemberMonitor(() => transform.localRotation, (val) => transform.localRotation = (Quaternion)val),
                new MemberMonitor(() => transform.localScale, (val) => transform.localScale = (Vector3)val)
            };
        }
    }
}
