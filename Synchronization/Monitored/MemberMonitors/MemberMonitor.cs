using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public class MemberMonitor<T>: AMemberMonitorBase, IMemberMonitor<T>
    {
        protected readonly Func<T> _getValueFunc;
        protected readonly Action<T> _setValueFunc;

        public MemberMonitor(string name, Func<T> getValue, Action<T> setValue) : base(name)
        {
            _getValueFunc = getValue ?? throw new ArgumentNullException(nameof(getValue));
            _setValueFunc = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

        public override Type MemberType => typeof(T);

        public override object GetValue()
        {
            return _getValueFunc.Invoke();
        }

        public override void SetValue(object obj)
        {
            _setValueFunc.Invoke((T)obj);
        }
    }
}
