using System.Collections.Generic;
using UnityEngine;

namespace UnityIntegration
{
    [AddComponentMenu(EditorConstants.ComponentMenuName + "/" + nameof(Synchronizer))]
    public class Synchronizer: MonoBehaviour
    {
        public SyncClientFilter ClientFilter;
        public List<MonoBehaviour> Behaviours;
    }
}
