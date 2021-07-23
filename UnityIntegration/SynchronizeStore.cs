using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    public class SynchronizeStore
    {
        public static SynchronizeStore Instance => _instance ?? (_instance = new SynchronizeStore());
        private static SynchronizeStore _instance;
        public Dictionary<int, Synchronizer> synchronizers;
        private int idCounter;
        public SynchronizeStore()
        {
            synchronizers = new Dictionary<int, Synchronizer>();
        }

        internal void Register(Synchronizer synchronizer)
        {
            synchronizer.SynchronizerId = idCounter++;
            synchronizers.Add(synchronizer.SynchronizerId, synchronizer);
        }
        internal void Unregister(Synchronizer synchronizer) { synchronizers.Remove(synchronizer.SynchronizerId); }
    }
}
