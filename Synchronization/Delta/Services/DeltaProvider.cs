using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstantMultiplayer.Synchronization.Delta.Services
{
    public sealed class DeltaProvider
    {
        public bool TryGetDeltaComponent(ComponentMonitor monitoredComponent, float timeStamp, out DeltaComponent deltaComponent)
        {
            try
            {
                var members = GetDeltaMembers(monitoredComponent.Members, timeStamp)?.ToArray();
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

        public IEnumerable<DeltaMember> GetDeltaMembers(IList<AMemberMonitorBase> members, float timeStamp)
        {
            var deltaMembers = new List<DeltaMember>();
            for(int i=0; i< members.Count; i++)
            {
                var member = members[i];
                if (!member.TryGetDelta(out var val))
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
