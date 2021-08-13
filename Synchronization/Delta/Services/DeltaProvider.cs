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

        public IEnumerable<DeltaMember> GetDeltaMembers(ComponentMonitor monitoredComponent, float timeStamp)
        {
            var deltaMembers = new List<DeltaMember>();
            for(int i=0; i<monitoredComponent.Members.Count; i++)
            {
                var member = monitoredComponent.Members[i];
                object val;
                if (member is ARichMemberMonitorBase richMemberMonitor)
                {
                    var localCompareVal = richMemberMonitor.GetLocalCompareValue();
                    if ((richMemberMonitor.LastLocalCompareValue == null && localCompareVal == null) || 
                        (richMemberMonitor.LastLocalCompareValue != null && richMemberMonitor.LastLocalCompareValue.Equals(localCompareVal)))
                        continue;
                    richMemberMonitor.LastLocalCompareValue = localCompareVal;
                    val = member.GetValue();
                }
                else
                {
                    val = member.GetValue();
                    if ((member.LastValue == null && val == null) || (member.LastValue != null && member.LastValue.Equals(val)))
                        continue;
                }
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
