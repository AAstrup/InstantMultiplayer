namespace SynchronizationFR
{
    public sealed class DeltaConsumer
    {
        public void ConsumeDelta(DeltaComponent deltaComponent, MonitoredComponent monitoredComponent)
        {
            for(int i=0; i<monitoredComponent.Members.Length; i++)
            {
                if (monitoredComponent.Members[i].LastUpdateTimestamp > deltaComponent.Members[i].TimeStamp)
                    continue;
                monitoredComponent.Members[i].SetValue(deltaComponent.Members[i]);
                monitoredComponent.Members[i].LastValue = deltaComponent.Members[i];
                monitoredComponent.Members[i].LastUpdateTimestamp = deltaComponent.Members[i].TimeStamp;
            }
        }
    }
}
