using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Delta.Services
{
    public sealed class DeltaConsumer
    {
        public void ConsumeDelta(DeltaComponent deltaComponent, ComponentMonitor monitoredComponent)
        {
            var e = 0;
            for(int i=0; i < monitoredComponent.Members.Count && e < deltaComponent.Members.Length; i++)
            {
                var monitoredMember = monitoredComponent.Members[i];
                var deltaMember = deltaComponent.Members[e];
                if (deltaMember.Index != i)
                    continue;
                e++;
                if (monitoredMember.LastUpdateTimestamp > deltaMember.TimeStamp)
                    continue;
                monitoredMember.SetUpdatedValue(deltaMember.Value, deltaMember.TimeStamp);
                //if (monitoredMember is ARichMemberMonitorBase richMemberMonitor)
                //    richMemberMonitor.LastLocalCompareValue = richMemberMonitor.GetLocalCompareValue();
                if (monitoredMember.OnDeltaConsumed != null)
                    monitoredMember.OnDeltaConsumed.Invoke(this, deltaMember);
            }
        }
    }
}
