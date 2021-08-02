using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    public class SynchronizeStore
    {
        public static SynchronizeStore Instance => _instance ?? (_instance = new SynchronizeStore());
        private static SynchronizeStore _instance;

        public Dictionary<int, Synchronizer> synchronizers;
        public int LocalId { get; private set; }

        private int _idCounter;

        public SynchronizeStore()
        {
            synchronizers = new Dictionary<int, Synchronizer>();
            _idCounter = 1;
        }

        internal void Register(Synchronizer synchronizer, bool foreign)
        {
            if (!foreign)
            {
                //The first 5 bits are reserved for player id
                synchronizer.SynchronizerId = (_idCounter << 5) + LocalId;
                _idCounter++;
            }
            synchronizers.Add(synchronizer.SynchronizerId, synchronizer);
        }
        internal void Unregister(Synchronizer synchronizer) { synchronizers.Remove(synchronizer.SynchronizerId); }
        internal void DigestLocalId(int localId)
        {
            LocalId = localId;
            var syncs = synchronizers.Values.ToList();
            foreach (var sync in syncs)
                sync.SynchronizerId = (sync.SynchronizerId & (-1 << 5)) + LocalId;
            synchronizers = syncs.ToDictionary(s => s.SynchronizerId, s => s);
        }
    }
}
