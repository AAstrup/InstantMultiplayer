using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.Synchronization.Extensions;
using System;
using System.Collections.Generic;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public abstract class AMemberMonitorBase: IMemberMonitorBase
    {
        public string Name { get; protected set; }
        public float LastUpdateTimestamp { get; internal set; }
        public EventHandler<DeltaMember> OnDeltaConsumed { get; set; }

        public abstract object GetValue();
        public abstract void SetValue(object obj);
        public object LastValue { get; set; }

        public abstract Type MemberType { get; }

        internal List<IDeltaMemberSuppressor> _suppressors;

        public AMemberMonitorBase(string name)
        {
            Name = name;
        }

        public virtual void SetUpdated(object obj, float timeStamp)
        {
            LastValue = obj;
            LastUpdateTimestamp = timeStamp;
        }

        public void AddSuppressor(IDeltaMemberSuppressor suppressor)
        {
            if (_suppressors == null)
                _suppressors = new List<IDeltaMemberSuppressor>();
            _suppressors.Add(suppressor);
        }

        public override string ToString()
        {
            return $"Monitored {MemberType}: {LastValue}";
        }

        public virtual bool TryGetDelta(out object delta)
        {
            delta = GetValue();
            if ((LastValue == null && delta == null) || (LastValue != null && LastValue.Equals(delta)))
                return false;
            return true;
        }

        public virtual void ConsumeDelta(object delta, float timeStamp)
        {
            this.SetUpdatedValue(delta, timeStamp);
        }
    }
}
