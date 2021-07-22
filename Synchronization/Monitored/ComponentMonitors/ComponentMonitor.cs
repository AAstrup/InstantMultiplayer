using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors
{
    public sealed class ComponentMonitor
    {
        public readonly int Id;
        public readonly MemberMonitor[] Members;

        public ComponentMonitor(int id, MemberMonitor[] fields)
        {
            Id = id;
            Members = fields ?? throw new ArgumentNullException(nameof(fields));
        }
    }
}
