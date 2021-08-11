using InstantMultiplayer.Synchronization.Delta;
using System;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation
{
    public abstract class ASyncMemberInterpolator<T> : ASyncMemberInterpolatorBase
    {
        public TimedValue<T> LastTimedValue { get; set; }
        public TimedValue<T> TimedValue { get; set; }
        public override Type GenericType => typeof(T);

        public override void HandleDeltaMember(DeltaMember deltaMember)
        {
            LastTimedValue = TimedValue;
            TimedValue = new TimedValue<T>
            {
                Value = (T)deltaMember.Value,
                Timestamp = Time.time //deltaMember.TimeStamp
            };
        }

        internal void Update()
        {
            if (_memberMonitorBase == null)
                return;
            _memberMonitorBase.SetValue(Interpolate((T)_memberMonitorBase.GetValue()));
        }

        public abstract T Interpolate(T localValue);
    }
}
