using System.Collections.Generic;
using System.Linq;

namespace SynchronizationFR.Delta
{
    public sealed class DeltaProvider
    {
        public bool TryGetDeltaComponent(MonitoredComponent monitoredComponent, int timeStamp, out DeltaComponent deltaComponent)
        {
            var members = GetDeltaMembers(monitoredComponent, timeStamp).ToArray();
            if (members.Length == 0)
            {
                deltaComponent = null;
                return false;
            }
            deltaComponent = new DeltaComponent
            {
                Id = monitoredComponent.Id,
                Members = members
            };
            return true;
        }

        public IEnumerable<DeltaMember> GetDeltaMembers(MonitoredComponent monitoredComponent, int timeStamp)
        {
            for(int i=0; i<monitoredComponent.Members.Length; i++)
            {
                var member = monitoredComponent.Members[i];
                var val = member.GetValue();
                if (member.LastValue.Equals(val))
                    continue;
                member.LastValue = val;
                member.LastUpdateTimestamp = timeStamp;
                yield return new DeltaMember
                {
                    Index = i,
                    Value = val,
                    TimeStamp = timeStamp
                };
            }
        }
    }
}
