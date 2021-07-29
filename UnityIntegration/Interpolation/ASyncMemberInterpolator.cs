using InstantMultiplayer.Synchronization.Delta;
using System;

namespace InstantMultiplayer.UnityIntegration.Interpolation
{
    public abstract class ASyncMemberInterpolator<T> : ASyncMemberInterpolatorBase
    {
        public TimedValue<T> LastTimedValue { get; set; }
        public TimedValue<T> TimedValue { get; set; }
        public override Type GenericType => typeof(T);

        internal override void DeltaConsumeHandler(DeltaMember deltaMember)
        {
            LastTimedValue = TimedValue;
            TimedValue = new TimedValue<T>
            {
                Value = (T)deltaMember.Value,
                Timestamp = deltaMember.TimeStamp
            };
        }
    }
}
