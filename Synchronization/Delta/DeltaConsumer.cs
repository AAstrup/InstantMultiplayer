﻿using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;

namespace InstantMultiplayer.Synchronization.Delta
{
    public sealed class DeltaConsumer
    {
        public void ConsumeDelta(DeltaComponent deltaComponent, ComponentMonitor monitoredComponent)
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