using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation.Implementors
{
    public class Vector3Interpolator : ASyncMemberInterpolator<Vector3>
    {
        public void Update()
        {
            transform.position = LastTimedValue.Value + (TimedValue.Value - LastTimedValue.Value) * (Time.time - TimedValue.Timestamp);
        }
    }
}
