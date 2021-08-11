using InstantMultiplayer.UnityIntegration;
using KrisCorner.Utilities.Extensions;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.DemoGames.TankGame
{
    public class TankSpawner: MonoBehaviour
    {
        public GameObject TankPrefab;
        private GameObject _tankInstance;

        public Arena Arena;

        public void Update()
        {
            if (!SyncClient.Instance.Ready)
                return;

            if(_tankInstance == null && Input.GetKeyDown(KeyCode.Space))
            {
                var spawn = Arena.Spawns.OrderBy(s => Guid.NewGuid()).First();
                if (spawn == null)
                    return;
                _tankInstance = UnityEngine.Object.Instantiate(TankPrefab);
                if (_tankInstance == null)
                    return;
                _tankInstance.transform.position = spawn.transform.position.Y(0.5f);
                //var controller = _tankInstance.GetComponent<TankController>();
                //controller.enabled = true;
            }
        }
    }
}
