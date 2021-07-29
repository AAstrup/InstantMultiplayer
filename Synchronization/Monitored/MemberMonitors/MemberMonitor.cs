using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public class MemberMonitor<T>: AMemberMonitorBase
    {
        public override object LastValue { get { return TypedLastValue; } set { TypedLastValue = (T)value;  } }
        public T TypedLastValue { get; private set; }

        public readonly Func<T> GetValueFunc;
        public readonly Action<T> SetValueFunc;

        public MemberMonitor(string name, Func<T> getValue, Action<T> setValue) : base(name)
        {
            GetValueFunc = getValue ?? throw new ArgumentNullException(nameof(getValue));
            SetValueFunc = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

        public override Type MemberType => typeof(T);

        public override object GetValue()
        {
            return GetValueFunc.Invoke();
        }

        public override void SetValue(object obj)
        {
            SetValueFunc.Invoke((T)obj);
        }
    }
}
