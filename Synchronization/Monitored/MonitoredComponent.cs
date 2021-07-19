using System;

namespace InstantMultiplayer.Synchronization.Monitored
{
    public class MonitoredComponent
    {
        public readonly string Id;
        public readonly MonitoredMember[] Fields;

        public MonitoredComponent(string id, MonitoredMember[] fields)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }
    }
}
