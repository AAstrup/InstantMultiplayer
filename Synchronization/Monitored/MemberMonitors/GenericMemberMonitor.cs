using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public class GenericMemberMonitor: AMemberMonitorBase
    {
        public readonly Type Type;
        public readonly Func<object> GetValueFunc;
        public readonly Action<object> SetValueFunc;

        public GenericMemberMonitor(string name, Type type, Func<object> getValue, Action<object> setValue) : base(name)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            GetValueFunc = getValue ?? throw new ArgumentNullException(nameof(getValue));
            SetValueFunc = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

        public override Type MemberType => Type;

        public override object GetValue()
        {
            return GetValueFunc.Invoke();
        }

        public override void SetValue(object obj)
        {
            SetValueFunc.Invoke(obj);
        }
    }
}
