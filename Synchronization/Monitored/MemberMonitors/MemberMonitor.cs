using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public sealed class MemberMonitor
    {
        public readonly string Name;
        public readonly Func<object> GetValue;
        public readonly Action<object> SetValue;
        public object LastValue { get; internal set; }
        public int LastUpdateTimestamp { get; internal set; }

        public MemberMonitor(string name, Func<object> getValue, Action<object> setValue)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            GetValue = getValue ?? throw new ArgumentNullException(nameof(getValue));
            SetValue = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

        public override string ToString()
        {
            return "Monitored: " + LastValue.ToString();
        }
    }
}
