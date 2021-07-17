using UnityEngine;

namespace UnityIntegration
{
    [CreateAssetMenu(fileName = nameof(SyncClientFilter), menuName = EditorConstants.MenuName + "/" + nameof(SyncClientFilter), order = 1)]
    public class SyncClientFilter: ScriptableObject
    {
        public int ClientFilter;
    }
}
