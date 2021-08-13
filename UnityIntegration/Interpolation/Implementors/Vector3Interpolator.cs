using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation.Implementors
{
    public class Vector3Interpolator : ASyncMemberInterpolator<Vector3>
    {
        private Vector3 _lerpingValue;

        public override Vector3 Interpolate(Vector3 localValue)
        {
            var timeSince = Time.time - TimedValue.LocalTimeStamp;
            var timeDelta = TimedValue.LocalTimeStamp - LastTimedValue.LocalTimeStamp;
            if (timeSince > timeDelta * 1.5f)
                return TimedValue.Value;
            if (!LocalLerping)
                return LastTimedValue.Value + (timeSince / timeDelta) * (TimedValue.Value - LastTimedValue.Value);
            else
            {
                _lerpingValue += (LastTimedValue.Value + (timeSince / timeDelta) * (TimedValue.Value - LastTimedValue.Value)
                    - _lerpingValue) * Time.deltaTime * LocalLerpScale;
                return _lerpingValue;
            }
                
        }
    }
}
