using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;

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
                monitoredMember.LastValue = deltaMember.Value;
                monitoredMember.LastUpdateTimestamp = deltaMember.TimeStamp;
            }
        }
    }
}
