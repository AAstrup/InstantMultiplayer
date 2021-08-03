using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors
{
    public sealed class ComponentMonitor
    {
        public readonly int Id;
        public readonly Component MonitoredInstance;
        public ReadOnlyCollection<AMemberMonitorBase> Members => Array.AsReadOnly(_members);

        internal readonly AMemberMonitorBase[] _members;

        internal ComponentMonitor(int id, Component instance, AMemberMonitorBase[] fields)
        {
            Id = id;
            MonitoredInstance = instance ?? throw new ArgumentNullException(nameof(instance));
            _members = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public override string ToString()
        {
            return "Monitored: " + MonitoredInstance.ToString();
        }
    }
}
