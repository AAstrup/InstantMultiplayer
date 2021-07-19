using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored
{
    public interface IMonitorProvider
    {
        IEnumerable<Type> ComponentTypes();
        IEnumerable<MonitoredMember> MonitoredMembers(Component componentInstance);
    }
}
