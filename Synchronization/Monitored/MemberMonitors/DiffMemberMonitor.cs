using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public class DiffMemberMonitor<T>: MemberMonitor<T>, IDiffMemberMonitor<T>
    {
        private T AccumulatedLocalValue;
        private T MonitoredValue;
        //private T LastLocalValue;
        //private T AccumulatedForeignDiffValue;
        private readonly Func<T, T, T> _plusOperation;
        private readonly Func<T, T, T> _minusOperation;
        private readonly Func<T, T, bool> _equalityOperation;

        public DiffMemberMonitor(string name, Func<T> getValue, Action<T> setValue, Func<T,T,T> plusOperation, Func<T,T,T> minusOperation, Func<T, T, bool> equalityOperation) : base(name, getValue, setValue)
        {
            if (Nullable.GetUnderlyingType(MemberType) != null)
                throw new Exception($"Generic type of DiffMemberMonitor must be value-type (non-nullable)!");
            _plusOperation = plusOperation ?? throw new ArgumentNullException(nameof(plusOperation));
            _minusOperation = minusOperation ?? throw new ArgumentNullException(nameof(minusOperation));
            _equalityOperation = equalityOperation ?? throw new ArgumentNullException(nameof(equalityOperation));
        }

        public override void ConsumeDelta(object delta, float timeStamp)
        {
            var deltaValue = (T)delta;
            /*AccumulatedForeignDiffValue = _plusOperation.Invoke(AccumulatedForeignDiffValue, deltaValue);
            var val = _getValueFunc.Invoke();
            var newValue = _plusOperation.Invoke(val, deltaValue);
            _setValueFunc?.Invoke(newValue);
            Debug.Log($"{deltaValue} + {val} = {newValue}");*/
            var val = _getValueFunc();
            var localDiff = _minusOperation(val, MonitoredValue);
            if(!_equalityOperation(localDiff, default))
            {
                AccumulatedLocalValue = _plusOperation(AccumulatedLocalValue, localDiff);
            }
            if(AccumulatedLocalValue.Equals(default))
            {
                //Should be safe to directly set value (not diff)
            }
            MonitoredValue = _plusOperation(val, deltaValue);
            _setValueFunc(MonitoredValue);
        }

        public override bool TryGetDelta(out object delta)
        {
            /*var val = _getValueFunc.Invoke();
            var acc = _plusOperation.Invoke(LastLocalValue, AccumulatedForeignDiffValue);
            if(_equalityOperation.Invoke(val, acc))
            {
                delta = null;
                return false;
            }
            LastLocalValue = val;
            AccumulatedForeignDiffValue = default;
            delta = _minusOperation.Invoke(val, acc);
            Debug.Log($"{val} - {acc} = {delta}");
            return true;*/
            var val = _getValueFunc();
            var localDiff = _minusOperation(val, MonitoredValue);
            if (!_equalityOperation(localDiff, default))
            {
                AccumulatedLocalValue = _plusOperation(AccumulatedLocalValue, localDiff);
            }
            if (AccumulatedLocalValue.Equals(default))
            {
                delta = null;
                return false;
            }
            MonitoredValue = val;
            delta = AccumulatedLocalValue;
            AccumulatedLocalValue = default;
            return true;
        }
    }
}
