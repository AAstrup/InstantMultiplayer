using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;

namespace InstantMultiplayer.Synchronization.Delta
{
    public sealed class DeltaConsumer
    {
        public void ConsumeDelta(DeltaComponent deltaComponent, ComponentMonitor monitoredComponent)
        {
            var e = 0;
            for(int i=0; i < monitoredComponent.Members.Length && i < deltaComponent.Members.Length; i++)
            {
                var monitoredMember = monitoredComponent.Members[i];
                var deltaMember = deltaComponent.Members[e];
                if (deltaMember.Index != i)
                    continue;
                e++;
                if (monitoredMember.LastUpdateTimestamp > deltaMember.TimeStamp)
                    continue;
                monitoredMember.SetValue(deltaMember.Value);
                if (monitoredMember.OnDeltaConsumed != null)
                    monitoredMember.OnDeltaConsumed.Invoke(this, deltaMember);
                if (monitoredMember is ARichMemberMonitorBase richMemberMonitor)
                    richMemberMonitor.LastValue = richMemberMonitor.GetLocalCompareValue();
                else
                    monitoredMember.LastValue = deltaMember.Value;
                monitoredMember.LastUpdateTimestamp = deltaMember.TimeStamp;
            }
        }
    }
}
