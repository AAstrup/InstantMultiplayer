using InstantMultiplayer.Synchronization.Objects;
using InstantMultiplayer.UnityIntegration;
using System;
using System.IO;
using UnityEngine;

namespace Synchronization.Objects.Resources
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(ResourceOutline), menuName = EditorConstants.AssetMenuName + "/" + nameof(ResourceOutline), order = 2)]
    public class ResourceOutline: ScriptableObject
    {
        public bool Outdated;
        public ResourceEntry[] Entries;

        public static string Name => "ResourceOutline";
        public static string AssetPath => Path.Combine("Assets", "Resources", Name + ".asset");
        public static string ResourcePath => Name;

        public ResourceOutline() 
        {
            Entries = new ResourceEntry[0];
        }
    }
}
