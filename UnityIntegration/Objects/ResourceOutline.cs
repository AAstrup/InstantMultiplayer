using InstantMultiplayer.Synchronization.Objects;
using System;
using System.IO;
using UnityEngine;

namespace Synchronization.Objects.Resources
{
    [Serializable]
    public class ResourceOutline: ScriptableObject
    {
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
