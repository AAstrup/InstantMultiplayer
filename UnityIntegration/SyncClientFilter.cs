using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(SyncClientFilter), menuName = EditorConstants.AssetMenuName + "/" + nameof(SyncClientFilter), order = 1)]
    public sealed class SyncClientFilter: ScriptableObject
    {
        public int ClientFilter;

        public SyncClientFilter() { }

        public SyncClientFilter(int clientFilter)
        {
            ClientFilter = clientFilter;
        }

        public SyncClientFilter(IEnumerable<int> includedClients)
        {
            var filter = 0;
            foreach(var includedClient in includedClients)
                filter |= 1 << includedClient;
            ClientFilter = filter;
        }

        public SyncClientFilter(params int[] includedClients)
        {
            var filter = 0;
            foreach (var includedClient in includedClients)
                filter |= 1 << includedClient;
            ClientFilter = filter;
        }

        public SyncClientFilter(params bool[] clientInclusions)
        {
            var filter = 0;
            for (int i = 0; i < 32 && i < clientInclusions.Length; i++)
                filter |= 1 << i;
            ClientFilter = filter;
        }
    }
}
