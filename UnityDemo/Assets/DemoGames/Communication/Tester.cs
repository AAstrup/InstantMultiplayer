using InstantMultiplayer.UnityIntegration;
using KrisCorner.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DemoGames.Communication
{
    public class Tester: MonoBehaviour
    {
        private bool _moveRight;

        public float MoveSpeed;

        private void Start()
        {
            SyncClient.Instance.OnIdentified += (s, m) => {
                if (m.LocalId != 1)
                    Destroy(gameObject);
            };
        }

        public void Update()
        {
            if (!SyncClient.Instance.Ready)
                return;

            var hor = Input.GetAxisRaw("Horizontal");
            var ver = Input.GetAxisRaw("Vertical");

            if (_moveRight)
            {
                hor = 1;
            }

            if (hor != 0 || ver != 0)
            {
                transform.position += (Vector3.right * hor + Vector3.forward * ver) * MoveSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x.MinMax(-4.5f, 4.5f), transform.position.y, transform.position.z.MinMax(-4.5f, 4.5f));
            }

            if (Input.GetKeyDown(KeyCode.Space))
                _moveRight = !_moveRight;
              
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                transform.position += Vector3.right * -5;
            }
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent<Synchronizer>(out _))
                return;
            if(collision.transform.position.x < transform.position.x)
                collision.transform.position += Vector3.right * -5;
        }*/
    }
}
