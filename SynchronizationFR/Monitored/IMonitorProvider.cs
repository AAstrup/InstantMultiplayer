using System;
using System.Collections.Generic;
using UnityEngine;

namespace SynchronizationFR.Monitored
{
    public interface IMonitorProvider
    {
        IEnumerable<Type> ComponentTypes();
        IEnumerable<MonitoredMember> MonitoredMembers(Component componentInstance);
    }
}
