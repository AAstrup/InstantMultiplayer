using InstantMultiplayer.Synchronization.Identification;
using InstantMultiplayer.Synchronization.Objects;
using Synchronization.Objects.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor.ResourceOutlines
{
    internal static class ResourceOutlineHelper
    {
        internal static void MarkResourceOutlineAsOutdated()
        {
            var resourceOutline = GetOrCreate(out var created);
            if (resourceOutline.Outdated)
                return;
            resourceOutline.Outdated = true;
            EditorUtility.SetDirty(resourceOutline);
            //AssetDatabase.SaveAssets();
            //if (created)
            //    AssetDatabase.Refresh();
        }

        internal static bool ShouldUpdate(string assetName, bool checkMeta = true)
        {
            if (checkMeta && assetName.EndsWith(".meta"))
                return false;
            var assetPath = Path.GetFullPath(assetName);
            var resourceOutlineAssetPath = Path.GetFullPath(ResourceOutline.AssetPath);
            return string.Compare(assetPath, resourceOutlineAssetPath, System.StringComparison.InvariantCultureIgnoreCase) != 0;
        }

        internal static void UpdateResourceOutline()
        {
            var resourceOutline = GetOrCreate(out var created);
            //if (!resourceOutline.Outdated)
            //    return;
            var entries = GetEntries();
            resourceOutline.Entries = entries.ToArray();
            resourceOutline.Outdated = false;
            EditorUtility.SetDirty(resourceOutline);
            AssetDatabase.SaveAssets();
            if (created)
                AssetDatabase.Refresh();
        }

        private static ResourceOutline GetOrCreate(out bool created)
        {
            var path = ResourceOutline.AssetPath;
            var resourceOutline = AssetDatabase.LoadAssetAtPath<ResourceOutline>(path);
            created = false;
            if (resourceOutline == null)
            {
                resourceOutline = ScriptableObject.CreateInstance<ResourceOutline>();
                AssetDatabase.CreateAsset(resourceOutline, path);
                Debug.Log("Created new ResourceOutline asset");
                created = true;
            }
            return resourceOutline;
        }

        private static List<ResourceEntry> GetEntries()
        {
            var entries = new List<ResourceEntry>();
            var resourcesPath = Path.Combine(Application.dataPath, "Resources");
            var assetResourcesIndexOffset = Path.Combine("Assets, Resources").Length;
            foreach (string d in Directory.GetDirectories(resourcesPath))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    if (f.EndsWith(".meta"))
                        continue;
                    var index = f.IndexOf("Assets");
                    var assetPath = f.Substring(index);
                    var assetObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
                    if (assetObject == null)
                        continue;
                    var id = IdFactory.Instance.GetId(assetObject);
                    var resourcePath = f.Substring(index + assetResourcesIndexOffset);
                    var finalSegment = resourcePath.Split('/', '\\').Last();
                    var suffixLength = finalSegment.Contains(".") ? finalSegment.Split('.', '.').Last().Length + 1 : 0;
                    resourcePath = resourcePath.Substring(0, resourcePath.Length - suffixLength);
                    entries.Add(new ResourceEntry
                    {
                        Name = assetObject.name,
                        Id = id,
                        TypeName = assetObject.GetType().FullName,
                        Path = resourcePath,
                    });
                }
            }
            return entries;
        }
    }
}
