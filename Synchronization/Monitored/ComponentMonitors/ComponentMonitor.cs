using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors
{
    public sealed class ComponentMonitor: IComponentMonitor
    {
        public int Id { get; private set; }
        public Component MonitoredInstance { get; private set; }
        public ReadOnlyCollection<AMemberMonitorBase> Members => Array.AsReadOnly(_members);

        private readonly AMemberMonitorBase[] _members;

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
