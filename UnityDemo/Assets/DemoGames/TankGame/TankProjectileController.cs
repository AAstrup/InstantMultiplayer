﻿using InstantMultiplayer.UnityIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    [RequireComponent(typeof(TankProjectile))]
    public class TankProjectileController: MonoBehaviour
    {
        private TankProjectile _data;
        private void Start()
        {
            _data = GetComponent<TankProjectile>();
            CalculatePosition();
        }
        private void Update()
        {
            try
            {
                transform.position = CalculatePosition();
            }
            catch (Exception) { }
            var timeDelta = SyncClient.Instance.SyncTime - _data.CreatedTimestamp;
            if (timeDelta >= _data.Duration && _data.OwnerId == SyncClient.Instance.LocalId)
            {
                _data.Tank.ShotsLeft += 1;
                Destroy(gameObject);
            }
        }
        private Vector3 CalculatePosition()
        {
            var positionDelta = _data.End - _data.Start;
            var timeDelta = SyncClient.Instance.SyncTime - _data.CreatedTimestamp;
            return (timeDelta / _data.Duration) * positionDelta + _data.Start;
        }
    }
}
