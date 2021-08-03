using System.Collections.Generic;
using System.Linq;

namespace InstantMultiplayer.UnityIntegration
{
    public class SynchronizeStore
    {
        public static SynchronizeStore Instance => _instance ?? (_instance = new SynchronizeStore());
        private static SynchronizeStore _instance;

        private Dictionary<int, Synchronizer> _synchronizers;
        private int _localClientId;
        private int _idCounter;
        private HashSet<int> _exhaustedIds;

        public SynchronizeStore()
        {
            _synchronizers = new Dictionary<int, Synchronizer>();
            _idCounter = 1;
            _exhaustedIds = new HashSet<int>();
        }

        public IEnumerable<Synchronizer> Synchronizers => _synchronizers.Values;

        public bool TryGet(int synchronizerId, out Synchronizer synchronizer)
        {
            return _synchronizers.TryGetValue(synchronizerId, out synchronizer);
        }

        internal void Register(Synchronizer synchronizer, bool foreign)
        {
            if (!foreign)
            {
                //The first 5 bits are reserved for player id
                synchronizer.SynchronizerId = (_idCounter << 5) + _localClientId;
                _idCounter++;
            }
            _synchronizers.Add(synchronizer.SynchronizerId, synchronizer);
        }
        internal void Unregister(Synchronizer synchronizer) { 
            _synchronizers.Remove(synchronizer.SynchronizerId);
            _exhaustedIds.Add(synchronizer.SynchronizerId);
        }
        internal void DigestLocalClientId(int localId)
        {
            _localClientId = localId;
            var syncs = _synchronizers.Values.ToList();
            foreach (var sync in syncs)
                sync.SynchronizerId = (sync.SynchronizerId & (-1 << 5)) + _localClientId;
            _synchronizers = syncs.ToDictionary(s => s.SynchronizerId, s => s);
        }
        internal void ExhaustId(int synchronizerId)
        {
            _exhaustedIds.Add(synchronizerId);
        }
        internal bool IsIdExhausted(int synchronizerId)
        {
            return _exhaustedIds.Contains(synchronizerId);
        }
    }
}
