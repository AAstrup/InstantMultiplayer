using System;

namespace InstantMultiplayer.Synchronization.Monitored
{
    public class MonitoredMember
    {
        public readonly Func<object> GetValue;
        public readonly Action<object> SetValue;
        public object LastValue { get; internal set; }
        public int LastUpdateTimestamp { get; internal set; }

        public MonitoredMember(Func<object> getValue, Action<object> setValue)
        {
            GetValue = getValue ?? throw new ArgumentNullException(nameof(getValue));
            SetValue = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

    }
}
