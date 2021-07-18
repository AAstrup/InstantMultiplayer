using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    [AddComponentMenu(EditorConstants.ComponentMenuName + "/" + nameof(Synchronizer))]
    public class Synchronizer: MonoBehaviour
    {
        public SyncClientFilter ClientFilter;
        public List<MonoBehaviour> Behaviours;
    }
}
