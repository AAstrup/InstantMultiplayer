using InstantMultiplayer.UnityIntegration;
using KrisCorner.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DemoGames.TankGame
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Tank))]
    public class TankController: MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public Tank Tank;
        public Rigidbody Rigidbody;

        private Text text;
        private Camera _camera;

        private void Start()
        {
            text = GameObject.Find("Debug").GetComponent<Text>();
            _camera = Camera.main;
        }

        private void Update()
        {
            text.text = SyncClient.Instance.SyncTime.ToString();

            var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Rigidbody.velocity = dir * 3.5f;

            var targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition.Z(_camera.transform.position.y));
            var targetDir = (targetPosition - transform.position).Y(0).normalized;
            transform.rotation = Quaternion.Euler(0, new Vector2(targetDir.x, targetDir.z).ToAngle(), 0);

            var shotsLeftScale = (Tank.ShotsLeft + 3f) / (Tank.ShotsTotal + 3);
            transform.localScale = new Vector3(shotsLeftScale, 1, shotsLeftScale);

            if (Tank.ShotsLeft > 0 && Input.GetMouseButtonDown(0))
            {
                var originPosition = transform.position + targetDir * 0.71f;
                if (Physics.Raycast(new Ray(originPosition, targetDir), out var info, 100, 1<<LayerMask.NameToLayer("Wall")))
                {
                    Tank.ShotsLeft -= 1;
                    var projectile = SyncObject.Instantiate(ProjectilePrefab).GetComponent<TankProjectile>();
                    projectile.CreatedTimestamp = SyncClient.Instance.SyncTime;
                    projectile.Start = originPosition;
                    projectile.End = info.point;
                    projectile.Tank = Tank;
                    projectile.Duration = Vector3.Distance(info.point, originPosition) / 7f;
                    if(projectile.Duration == 0)
                    {
                        Debug.Log("!");
                    }
                    projectile.transform.position = projectile.Start + targetDir;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent<TankProjectileController>(out var projectile))
                return;
            //if (projectile.Synchronizer.OwnerId == SyncClient.Instance.LocalId)
            //    return;
            Destroy(gameObject);
        }
    }
}
