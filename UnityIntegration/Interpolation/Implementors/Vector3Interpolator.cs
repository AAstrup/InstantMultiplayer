using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation.Implementors
{
    public class Vector3Interpolator : ASyncMemberInterpolator<Vector3>
    {
        private Vector3 _lerpingValue;

        public override bool IsMemberSuppressed => enabled;

        public override Vector3 Interpolate(Vector3 localValue)
        {
            var timeSince = Time.time - TimedValue.LocalTimeStamp;
            var timeDelta = TimedValue.LocalTimeStamp - LastTimedValue.LocalTimeStamp;
            Debug.Log(timeSince);
            if (timeSince > timeDelta * 1.5f)
                return TimedValue.Value;
            var target = LastTimedValue.Value + (timeSince / timeDelta) * (TimedValue.Value - LastTimedValue.Value);
            if (!LocalLerping)
                return target;
            else
            {
                _lerpingValue += (target - _lerpingValue) * Time.deltaTime * LocalLerpScale;
                return _lerpingValue;
            }
                
        }
    }
}
