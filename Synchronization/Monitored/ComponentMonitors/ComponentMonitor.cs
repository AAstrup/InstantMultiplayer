using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors
{
    public sealed class ComponentMonitor
    {
        public readonly int Id;
        public readonly AMemberMonitorBase[] Members;
        public readonly Component MonitoredInstance;

        internal ComponentMonitor(int id, Component instance, AMemberMonitorBase[] fields)
        {
            Id = id;
            MonitoredInstance = instance;
            Members = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public override string ToString()
        {
            return "Monitored: " + MonitoredInstance.ToString();
        }
    }
}
