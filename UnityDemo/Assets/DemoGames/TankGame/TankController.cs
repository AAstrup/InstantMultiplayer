using InstantMultiplayer.UnityIntegration;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    [RequireComponent(typeof(Tank))]
    public class TankController: MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public Tank Tank;
        private void Update()
        {
            if(Tank.ShotsLeft > 0 && Input.GetMouseButtonDown(0))
            {
                if(Physics.Raycast(new Ray(transform.position, Vector3.forward), out var info))
                {
                    Tank.ShotsLeft -= 1;
                    var projectile = Object.Instantiate<GameObject>(ProjectilePrefab).GetComponent<TankProjectile>();
                    projectile.CreatedTimestamp = SyncClient.Instance.SyncTime;
                    projectile.Start = transform.position;
                    projectile.End = info.point;
                    projectile.OwnerId = SyncClient.Instance.LocalId;
                    projectile.Tank = Tank;
                }
            }
        }
    }
}
