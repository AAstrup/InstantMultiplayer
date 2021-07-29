using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public class RichMemberMonitor<T, E>: ARichMemberMonitorBase
    {
        public override object LastLocalCompareValue { get { return TypedLastLocalCompareValue; } set { TypedLastLocalCompareValue = (E)value; } }
        public E TypedLastLocalCompareValue { get; internal set; }
        public override object LastValue { get { return TypedLastValue; } set { TypedLastValue = (T)value; } }
        public T TypedLastValue { get; private set; }
        public readonly Func<T> GetValueFunc;
        public readonly Func<E> GetLocalCompareValueFunc;
        public readonly Action<T> SetValueFunc;

        public override Type MemberType => typeof(T);

        public RichMemberMonitor(string name, Func<T> getValue, Func<E> getLocalCompareValue, Action<T> setValue) : base(name)
        {
            GetValueFunc = getValue ?? throw new ArgumentNullException(nameof(getValue));
            GetLocalCompareValueFunc = getLocalCompareValue ?? throw new ArgumentNullException(nameof(getLocalCompareValue));
            SetValueFunc = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

        public override object GetValue()
        {
            return GetValueFunc.Invoke();
        }

        public override object GetLocalCompareValue()
        {
            return GetLocalCompareValueFunc.Invoke();
        }

        public override void SetValue(object obj)
        {
            SetValueFunc.Invoke((T)obj);
        }
    }
}
