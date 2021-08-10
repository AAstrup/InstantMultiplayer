using InstantMultiplayer.Synchronization.Filtering;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Logic
{
    public sealed class SyncFilterBehaviourEnabled: MonoBehaviour
    {
        public SyncClientFilter SyncClientFilter;
        public List<Behaviour> Behaviours;

        public void Start()
        {
            foreach (var beh in Behaviours)
                beh.enabled = false;
            SyncClient.Instance.OnIdentified += (s, e) =>
            {
                if (ClientFilterHelper.ClientIncluded(SyncClientFilter.ClientFilter, SyncClient.Instance.LocalId))
                {
                    foreach (var beh in Behaviours)
                        beh.enabled = true;
                }
            };
        }
    }
}
