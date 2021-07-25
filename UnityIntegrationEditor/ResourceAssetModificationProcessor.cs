using Synchronization.Objects;
using Synchronization.Objects.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor
{
    public class ResourceAssetModificationProcessor: UnityEditor.AssetModificationProcessor
    {
        //assetName is entire path, e.g: Assets/Resources/Textures/Dummy.png.meta
        private static void OnWillCreateAsset(string assetName) 
        {
            try
            {
                if (!ShouldUpdate(assetName, false)) return;
                UpdateResourceOutline();
            }
            catch (Exception e) { Debug.LogWarning("ResourceOutline failed to be created or updated: " + e.ToString()); }
        }
        private static AssetDeleteResult OnWillDeleteAsset(string assetName, RemoveAssetOptions options)
        {
            try
            {
                if (ShouldUpdate(assetName))
                    UpdateResourceOutline();
            }
            catch(Exception e) { Debug.LogWarning("ResourceOutline failed to be created or updated: " + e.ToString()); }
            return AssetDeleteResult.DidNotDelete;
        }
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            try
            {
                UpdateResourceOutline();
            }
            catch (Exception e) { Debug.LogWarning("ResourceOutline failed to be created or updated: " + e.ToString()); }
            return AssetMoveResult.DidNotMove;
        }

        private static bool ShouldUpdate(string assetName, bool checkMeta = true)
        {
            if (checkMeta && assetName.EndsWith(".meta"))
                return false;
            var assetPath = Path.GetFullPath(assetName);
            var resourceOutlineAssetPath = Path.GetFullPath(ResourceOutline.AssetPath);
            return string.Compare(assetPath, resourceOutlineAssetPath, System.StringComparison.InvariantCultureIgnoreCase) != 0;
        }

        private static void UpdateResourceOutline()
        {
            var entries = GetEntries();
            var path = ResourceOutline.AssetPath;
            var resourceOutline = AssetDatabase.LoadAssetAtPath<ResourceOutline>(path);
            if(resourceOutline == null)
            {
                resourceOutline = ScriptableObject.CreateInstance<ResourceOutline>();
                AssetDatabase.CreateAsset(resourceOutline, path);
                Debug.Log("Created new ResourceOutline asset");
            }
            resourceOutline.Entries = entries.ToArray();
            EditorUtility.SetDirty(resourceOutline);
            AssetDatabase.SaveAssets();
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
                    var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
                    if (asset == null)
                        continue;
                    var resourcePath = f.Substring(index + assetResourcesIndexOffset);
                    var finalSegment = resourcePath.Split('/', '\\').Last();
                    var suffixLength = finalSegment.Contains(".") ? finalSegment.Split('.', '.').Last().Length + 1 : 0;
                    resourcePath = resourcePath.Substring(0, resourcePath.Length - suffixLength);
                    entries.Add(new ResourceEntry
                    {
                        Path = resourcePath,
                        Name = asset.name,
                        TypeName = asset.GetType().FullName
                    });
                }
            }
            return entries;
        }
    }
}
