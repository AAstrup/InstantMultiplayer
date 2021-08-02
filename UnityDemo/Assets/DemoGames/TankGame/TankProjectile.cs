using InstantMultiplayer.Synchronization.Attributes;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    public class TankProjectile: MonoBehaviour
    {
        [HideInInspector]
        public float Duration;
        [HideInInspector]
        public Vector2 Start;
        [HideInInspector]
        public Vector2 End;
        [HideInInspector]
        public float CreatedTimestamp;
        [HideInInspector]
        public int OwnerId;

        [HideInInspector]
        [ExcludeSync]
        public Tank Tank;
    }
}
