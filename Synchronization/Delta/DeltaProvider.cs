using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstantMultiplayer.Synchronization.Delta
{
    public sealed class DeltaProvider
    {
        public bool TryGetDeltaComponent(ComponentMonitor monitoredComponent, int timeStamp, out DeltaComponent deltaComponent)
        {
            try
            {
                var members = GetDeltaMembers(monitoredComponent, timeStamp)?.ToArray();
                if (members == null || members.Length == 0)
                {
                    deltaComponent = null;
                    return false;
                }
                deltaComponent = new DeltaComponent
                {
                    Id = monitoredComponent.Id,
                    TypeId = ComponentMapper.GetCIDFromType(monitoredComponent.MonitoredInstance.GetType()),
                    Members = members
                };
                return true;
            }
            catch(Exception)
            {
                deltaComponent = null;
                return false;
            }
        }

        public IEnumerable<DeltaMember> GetDeltaMembers(ComponentMonitor monitoredComponent, int timeStamp)
        {
            var deltaMembers = new List<DeltaMember>();
            for(int i=0; i<monitoredComponent.Members.Length; i++)
            {
                var member = monitoredComponent.Members[i];
                var val = member.GetValue();
                if ((member.LastValue == null && val == null) || (member.LastValue != null && member.LastValue.Equals(val)))
                    continue;
                member.LastValue = val;
                member.LastUpdateTimestamp = timeStamp;
                deltaMembers.Add(new DeltaMember
                {
                    Index = i,
                    Value = val,
                    TimeStamp = timeStamp
                });
            }
            return deltaMembers;
        }
    }
}
