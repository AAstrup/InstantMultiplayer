using InstantMultiplayer.UnityIntegration;
using System;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    [RequireComponent(typeof(TankProjectile))]
    [RequireComponent(typeof(Synchronizer))]
    public class TankProjectileController: MonoBehaviour
    {
        public TankProjectile Data;
        public Synchronizer Synchronizer;

        private void Start()
        {
            CalculatePosition();
        }
        private void Update()
        {
            try
            {
                var pos = CalculatePosition();
                if (!float.IsNaN(pos.x) && !float.IsNaN(pos.y) && !float.IsNaN(pos.z))
                {
                    transform.position = pos;
                }
            }
            catch (Exception) { }
            var timeDelta = SyncClient.Instance.SyncTime - Data.CreatedTimestamp;
            if (timeDelta >= Data.Duration && Synchronizer.OwnerId == SyncClient.Instance.LocalId)
            {
                Data.Tank.ShotsLeft += 1;
                Destroy(gameObject);
            }
        }
        private Vector3 CalculatePosition()
        {
            var positionDelta = Data.End - Data.Start;
            var timeDelta = SyncClient.Instance.SyncTime - Data.CreatedTimestamp;
            return (timeDelta / Data.Duration) * positionDelta + Data.Start;
        }
    }
}
