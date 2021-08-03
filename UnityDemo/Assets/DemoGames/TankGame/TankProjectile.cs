using InstantMultiplayer.Synchronization.Attributes;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    public class TankProjectile: MonoBehaviour
    {
        [HideInInspector]
        public float Duration;
        [HideInInspector]
        public Vector3 Start;
        [HideInInspector]
        public Vector3 End;
        [HideInInspector]
        public float CreatedTimestamp;
        [HideInInspector]
        public int OwnerId;

        internal Tank Tank;
    }
}
