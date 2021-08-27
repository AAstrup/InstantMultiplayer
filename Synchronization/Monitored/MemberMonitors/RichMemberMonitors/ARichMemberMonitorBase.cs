using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.RichMemberMonitors
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

        public override bool TryGetDelta(out object delta)
        {
            var localCompareVal = GetLocalCompareValue();
            if ((LastLocalCompareValue == null && localCompareVal == null) ||
                (LastLocalCompareValue != null && LastLocalCompareValue.Equals(localCompareVal)))
            {
                delta = null;
                return false;
            }
            LastLocalCompareValue = localCompareVal;
            delta = GetValue();
            return true;
        }
    }
}
