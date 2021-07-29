using InstantMultiplayer.Synchronization.Delta;
using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public abstract class AMemberMonitorBase
    {
        public string Name { get; protected set; }
        public int LastUpdateTimestamp { get; internal set; }
        public EventHandler<DeltaMember> OnDeltaConsumed;

        public abstract object GetValue();
        public abstract void SetValue(object obj);
        public abstract object LastValue { get; set; }

        public abstract Type MemberType { get; }

        public AMemberMonitorBase(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Monitored {MemberType}: {LastValue}";
        }
    }
}
