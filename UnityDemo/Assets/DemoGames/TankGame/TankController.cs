using InstantMultiplayer.UnityIntegration;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Tank))]
    public class TankController: MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public Tank Tank;
        public Rigidbody Rigidbody;

        private void Update()
        {


            var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Rigidbody.velocity = dir * 6;

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
                    projectile.Duration = Vector3.Distance(info.point, transform.position) / 7;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent<TankProjectile>(out var projectile))
                return;
            if (projectile.OwnerId == SyncClient.Instance.LocalId)
                return;
            Destroy(gameObject);
        }
    }
}
