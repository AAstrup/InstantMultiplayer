﻿using InstantMultiplayer.UnityIntegration;
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

        public void Update()
        {
            var hor = Input.GetAxisRaw("Horizontal");
            var ver = Input.GetAxisRaw("Vertical");

            if (_moveRight)
            {
                hor = 1;
            }

            if (hor != 0 || ver != 0)
            {
                transform.position += (Vector3.right * hor + Vector3.forward * ver) * MoveSpeed * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
                _moveRight = !_moveRight;

            transform.position = new Vector3(transform.position.x.MinMax(-4.5f, 4.5f), transform.position.y, transform.position.z.MinMax(-4.5f, 4.5f));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent<Rigidbody>(out _))
                return;
            if (!collision.gameObject.TryGetComponent<Synchronizer>(out _))
                return;
            if(collision.transform.position.x < transform.position.x)
                collision.transform.position += Vector3.right * -5;
        }
    }
}
