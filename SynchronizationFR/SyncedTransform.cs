using System;
using System.Collections.Generic;
using UnityEngine;

namespace SynchronizationFR
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
                new MonitoredMember(() => transform.localPosition, (val) => transform.localPosition = (Vector3)val),
                new MonitoredMember(() => transform.localRotation, (val) => transform.localRotation = (Quaternion)val),
                new MonitoredMember(() => transform.localScale, (val) => transform.localScale = (Vector3)val)
            };
        }
    }
}
