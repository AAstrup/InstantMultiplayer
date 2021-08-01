using InstantMultiplayer.Synchronization.Filtering;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Logic
{
    public sealed class SyncFilterEnabled: MonoBehaviour
    {
        public SyncClientFilter SyncClientFilter;
        public List<Behaviour> Behaviours;

        public void Start()
        {
            var enabled = ClientFilterHelper.ClientIncluded(SyncClientFilter.ClientFilter, SyncClient.Instance.LocalId);
            foreach (var beh in Behaviours)
                beh.enabled = enabled;
        }
    }
}
