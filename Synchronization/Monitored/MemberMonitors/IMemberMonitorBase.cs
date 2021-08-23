using InstantMultiplayer.Synchronization.Delta;
using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public interface IMemberMonitorBase : IMonitor
    {
        string Name { get; }
        float LastUpdateTimestamp { get; }
        EventHandler<DeltaMember> OnDeltaConsumed { get; set; }

        object GetValue();
        void SetValue(object obj);
        object LastValue { get; set; }

        Type MemberType { get; }

        void SetUpdatedValue(object obj, float timeStamp);
        void SetUpdated(object obj, float timeStamp);
    }
}
