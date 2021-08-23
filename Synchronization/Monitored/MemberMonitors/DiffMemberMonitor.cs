using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public class DiffMemberMonitor<T>: MemberMonitor<T>, IDiffMemberMonitor<T>
    {
        public T LastLocalValue { get; set; }
        public T AccumulatedForeignDiffValue { get; private set; }

        private Func<T, T, T> _plusOperation;
        private Func<T, T, T> _minusOperation;

        public DiffMemberMonitor(string name, Func<T> getValue, Action<T> setValue, Func<T,T,T> plusOperation, Func<T,T,T> minusOperation) : base(name, getValue, setValue)
        {
            _plusOperation = plusOperation;
            _minusOperation = minusOperation;
        }

        public void ConsumeDiffValue(T deltaValue)
        {
            AccumulatedForeignDiffValue = _plusOperation.Invoke(AccumulatedForeignDiffValue, deltaValue);
        }

        public T GetDiffValue()
        {
            return _minusOperation.Invoke(GetValueFunc.Invoke(), _plusOperation.Invoke(LastLocalValue, AccumulatedForeignDiffValue));
        }
    }
}
