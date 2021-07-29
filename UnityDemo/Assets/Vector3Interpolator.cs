using UnityEngine;

namespace Assets
{
    public class Vector3Interpolator : AVector3Interpolator
    {
        private void Start()
        {
            Debug.Log("DIO");
        }

        public void Update()
        {
            transform.position = LastValue + (CurrentValue - LastValue) * (Time.time - Timestamp);
        }
    }
}
