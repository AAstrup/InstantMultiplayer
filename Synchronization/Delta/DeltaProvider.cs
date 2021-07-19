using InstantMultiplayer.Synchronization.Monitored;
using System.Collections.Generic;
using System.Linq;

namespace InstantMultiplayer.Synchronization.Delta
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
            foreach(var member in monitoredComponent.Members)
            {
                var val = member.GetValue();
                if (member.LastValue.Equals(val))
                    continue;
                member.LastValue = val;
                member.LastUpdateTimestamp = timeStamp;
                yield return new DeltaMember
                {
                    Value = val,
                    TimeStamp = timeStamp
                };
            }
        }
    }
}
