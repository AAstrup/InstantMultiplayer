using InstantMultiplayer.Synchronization.Identification;
using InstantMultiplayer.Synchronization.Objects;
using Synchronization.Objects.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor.ResourceOutlines
{
    internal static class ResourceSpecifier
    {
        public static Dictionary<Type, List<Type>> baseToSpecificType = new Dictionary<Type, List<Type>>()
        {
            { typeof(Texture2D), new List<Type>() { typeof(Sprite) } }
        };

        internal static bool SpecificationRequired(UnityEngine.Object assetObject)
        {
            return baseToSpecificType.ContainsKey(assetObject.GetType());
        }

        internal static UnityEngine.Object ReloadSpecificResource(UnityEngine.Object assetObject, string assetPath)
        {
            var types = baseToSpecificType[assetObject.GetType()];
            foreach (var type in types)
            {
                var specification = AssetDatabase.LoadAssetAtPath(assetPath, type);
                if (specification != null)
                    return specification;
            }
            return assetObject;
        }
    }
}
