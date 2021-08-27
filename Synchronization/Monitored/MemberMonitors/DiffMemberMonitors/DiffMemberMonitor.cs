using System;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.DiffMemberMonitors
{
    public class DiffMemberMonitor<T>: MemberMonitor<T>
    {
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

        public bool TypedEquality(T a, T b)
        {
            return _equalityOperation(a, b);
        }

        public T TypedAddition(T a, T b)
        {
            return _plusOperation(a, b);
        }

        public T TypedSubtraction(T a, T b)
        {
            return _minusOperation(a, b);
        }
    }
}
