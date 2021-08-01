using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation.Implementors
{
    public class Vector3Interpolator : ASyncMemberInterpolator<Vector3>
    {
        public override Vector3 Interpolate(Vector3 localValue)
        {
            var timeSince = Time.time - TimedValue.Timestamp;
            var timeDelta = TimedValue.Timestamp - LastTimedValue.Timestamp;
            if (timeSince > timeDelta * 1.5f)
                return TimedValue.Value;
            if (LocalLerping)
                return LastTimedValue.Value + (timeSince / timeDelta) * (TimedValue.Value - LastTimedValue.Value);
            else
                return localValue +
                    (LastTimedValue.Value + (timeSince / timeDelta) * (TimedValue.Value - LastTimedValue.Value) - localValue)
                    * Time.deltaTime * LocalLerpScale;
        }
    }
}
