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

        bool TryGetDelta(out object delta);
        void SetUpdated(object obj, float timeStamp);
        void ConsumeDelta(object delta, float timeStamp);
    }
}
