using System;

namespace SynchronizationFR
{
    public class MonitoredComponent
    {
        public readonly int Id;
        public readonly MonitoredMember[] Members;

        public MonitoredComponent(int id, MonitoredMember[] fields)
        {
            Id = id;
            Members = fields ?? throw new ArgumentNullException(nameof(fields));
        }
    }
}
