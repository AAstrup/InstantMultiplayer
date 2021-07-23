using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    public class SynchronizeStore : MonoBehaviour
    {
        public static SynchronizeStore Instance;
        public Dictionary<int, Synchronizer> synchronizers;
        private int idCounter;

        private void Awake()
        {
            Instance = this;
        }

        internal void Register(Synchronizer synchronizer)
        {
            synchronizer.SynchronizerId = idCounter++;
            synchronizers.Add(synchronizer.SynchronizerId, synchronizer);
        }
        internal void Unregister(Synchronizer synchronizer) { synchronizers.Remove(synchronizer.SynchronizerId); }
    }
}
