using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System.Collections.Generic;

namespace InstantMultiplayer.Synchronization.Delta.Services
{
    public sealed class DeltaConsumer
    {
        public void ConsumeDelta(DeltaComponent deltaComponent, ComponentMonitor monitoredComponent)
        {
            ConsumeDelta(deltaComponent.Members, monitoredComponent.Members);
        }

        public void ConsumeDelta(IList<DeltaMember> deltaMembers, IList<AMemberMonitorBase> monitorMembers)
        {
            var e = 0;
            for (int i = 0; i < monitorMembers.Count && e < deltaMembers.Count; i++)
            {
                var monitoredMember = monitorMembers[i];
                var deltaMember = deltaMembers[e];
                if (deltaMember.Index != i)
                    continue;
                e++;
                if (monitoredMember.LastUpdateTimestamp > deltaMember.TimeStamp)
                    continue;
                monitoredMember.SetUpdatedValue(deltaMember.Value, deltaMember.TimeStamp);
                if (monitoredMember.OnDeltaConsumed != null)
                    monitoredMember.OnDeltaConsumed.Invoke(this, deltaMember);
            }
        }
    }
}
