using InstantMultiplayer.Synchronization.Extensions;
using System;
using System.Reflection;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.DiffMemberMonitors
{
    public abstract class DiffMemberMonitorBase<T>: AMemberMonitorBase, IMemberMonitor<T>
    {
        private int _accDeltas;
        private (DiffDelta<T>, float) _firstDelta;
        private T _accumulatedLocalValue;
        private T _monitoredValue;

        private readonly object _memberHolder;
        private readonly MemberInfo _memberInfo;

        public DiffMemberMonitorBase(string name, object memberHolder, MemberInfo memberInfo) : base(name)
        {
            if (Nullable.GetUnderlyingType(MemberType) != null)
                throw new Exception($"Generic type of DiffMemberMonitor must be value-type (non-nullable)!");
            _memberHolder = memberHolder;
            _memberInfo = memberInfo;
        }

        public override Type MemberType => typeof(T);

        public override void ConsumeDelta(object delta, float timeStamp)
        {
            var deltaValue = (DiffDelta<T>)delta;

            if(_accDeltas == 0)
            {
                _firstDelta = (deltaValue, timeStamp);
            }
            _accDeltas++;

            var val = (T)GetValue();
            var localDiff = TypedSubtraction(val, _monitoredValue);
            if (!TypedEquality(localDiff, default(T)))
            {
                _accumulatedLocalValue = TypedAddition(_accumulatedLocalValue, localDiff);
            }
            _monitoredValue = TypedAddition(val, deltaValue.Diff);
            //Debug.Log($"diff: {localDiff}, acc: {_accumulatedLocalValue} - newval {_monitoredValue}");
            this.SetUpdatedValue(_monitoredValue, timeStamp);
        }

        public override bool TryGetDelta(out object delta)
        {
            var val = (T)GetValue();
            var localDiff = TypedSubtraction(val, _monitoredValue);
            if (!TypedEquality(localDiff, default(T)))
            {
                _accumulatedLocalValue = TypedAddition(_accumulatedLocalValue, localDiff);
            }
            if (_accumulatedLocalValue.Equals(default(T)))
            {
                if(_accDeltas == 1 && _firstDelta.Item1 != null)
                {
                    //Debug.Log($"Sat direct firstDelta value of {_firstDelta}");
                    this.SetUpdatedValue(_firstDelta.Item1.Direct, _firstDelta.Item2);
                    _monitoredValue = _firstDelta.Item1.Direct;
                }
                delta = null;
            }
            else
            {
                _monitoredValue = val;
                delta = new DiffDelta<T>
                {
                    Diff = _accumulatedLocalValue,
                    Direct = val
                };
                //Debug.Log($"SEND! diff: {localDiff}, acc: {_accumulatedLocalValue} - newval {_monitoredValue}");
                _accumulatedLocalValue = default(T);
            }
            _accDeltas = 0;
            _firstDelta = (default(DiffDelta<T>), 0);
            return delta != null;
        }

        public override object GetValue()
        {
            return _memberInfo.GetValueFromMemberInfo(_memberHolder);
        }

        public override void SetValue(object obj)
        {
            _memberInfo.SetValueFromMemberInfo(_memberHolder, obj);
        }

        public abstract bool TypedEquality(T a, T b);
        public abstract T TypedAddition(T a, T b);
        public abstract T TypedSubtraction(T a, T b);
    }

    [Serializable]
    public class DiffDelta<T>
    {
        public T Diff;
        public T Direct;

        public override string ToString()
        {
            return $"(diff: {Diff}, direct: {Direct})";
        }
    }
}
