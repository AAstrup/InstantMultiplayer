using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.Implementation
{
    public sealed class SyncedTransform : IMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            yield return typeof(SyncedTransform);
        }

        public IEnumerable<MonitoredMember> MonitoredMembers(Component componentInstance)
        {
            var transform = (Transform)componentInstance;
            return new MonitoredMember[]
            {
                new MonitoredMember(nameof(Transform.localPosition), () => transform.localPosition, (val) => transform.localPosition = (Vector3)val),
                new MonitoredMember(nameof(Transform.localRotation), () => transform.localRotation, (val) => transform.localRotation = (Quaternion)val),
                new MonitoredMember(nameof(Transform.localScale), () => transform.localScale, (val) => transform.localScale = (Vector3)val)
            };
        }
    }
}
