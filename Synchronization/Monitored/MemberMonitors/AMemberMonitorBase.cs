using InstantMultiplayer.Synchronization.Delta;
using System;
using System.Collections.Generic;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public abstract class AMemberMonitorBase: IMemberMonitor
    {
        public string Name { get; protected set; }
        public float LastUpdateTimestamp { get; internal set; }
        public EventHandler<DeltaMember> OnDeltaConsumed { get; set; }

        public abstract object GetValue();
        public abstract void SetValue(object obj);
        public abstract object LastValue { get; set; }

        public abstract Type MemberType { get; }

        internal List<IDeltaMemberSuppressor> _suppressors;

        public AMemberMonitorBase(string name)
        {
            Name = name;
        }

        public void SetUpdatedValue(object obj, float timeStamp)
        {
            SetValue(obj);
            SetUpdated(obj, timeStamp);
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
    }
}
