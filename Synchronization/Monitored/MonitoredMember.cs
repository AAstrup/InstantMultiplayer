using System;

namespace InstantMultiplayer.Synchronization.Monitored
{
    public class MonitoredMember
    {
        public readonly string Identifier;
        public readonly Func<object> GetValue;
        public readonly Action<object> SetValue;

        public MonitoredMember(string identifier, Func<object> getValue, Action<object> setValue)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            GetValue = getValue ?? throw new ArgumentNullException(nameof(getValue));
            SetValue = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

        public object LastValue { get; internal set; }
        public int LastUpdateTimestamp { get; internal set; }
    }
}
