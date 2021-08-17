using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public abstract class ARichMemberMonitorBase: AMemberMonitorBase
    {
        public abstract object LastLocalCompareValue { get; set; }
        public abstract object GetLocalCompareValue();
        public ARichMemberMonitorBase(string name) : base(name)
        {
        }
        public override void SetUpdated(object obj, float timeStamp)
        {
            base.SetUpdated(obj, timeStamp);
            LastLocalCompareValue = GetLocalCompareValue();
        }
    }
}
