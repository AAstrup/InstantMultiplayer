using InstantMultiplayer.Synchronization.Filtering;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Logic
{
    public sealed class SyncFilterEnabled: MonoBehaviour
    {
        public SyncClientFilter SyncClientFilter;

        public void Start()
        {
            enabled = false;
            SyncClient.Instance.OnIdentified += (s, e) =>
            {
                if (ClientFilterHelper.ClientIncluded(SyncClientFilter.ClientFilter, SyncClient.Instance.LocalId))
                {
                    enabled = true;
                }
            };
        }
    }
}
