using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor.ResourceOutlines
{
    public class ResourceAssetModificationProcessor: UnityEditor.AssetModificationProcessor
    {
        //assetName is entire path, e.g: Assets/Resources/Textures/Dummy.png.meta
        /*private static void OnWillCreateAsset(string assetName) 
        {
            try
            {
                if (!ResourceOutlineHelper.ShouldUpdate(assetName, false)) return;
                ResourceOutlineHelper.MarkResourceOutlineAsOutdated();
            }
            catch (Exception e) { Debug.LogWarning("ResourceOutline failed to be created or updated: " + e.ToString()); }
        }
        private static string[] OnWillSaveAssets(string[] paths)
        {
            try
            {
                if (paths.Any(p => ResourceOutlineHelper.ShouldUpdate(p, false)))
                    ResourceOutlineHelper.MarkResourceOutlineAsOutdated();
            }
            catch (Exception e) { Debug.LogWarning("ResourceOutline failed to be created or updated: " + e.ToString()); }
            return paths;
        }
        private static AssetDeleteResult OnWillDeleteAsset(string assetName, RemoveAssetOptions options)
        {
            try
            {
                if (ResourceOutlineHelper.ShouldUpdate(assetName))
                    ResourceOutlineHelper.MarkResourceOutlineAsOutdated();
            }
            catch(Exception e) { Debug.LogWarning("ResourceOutline failed to be created or updated: " + e.ToString()); }
            return AssetDeleteResult.DidNotDelete;
        }
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            try
            {
                ResourceOutlineHelper.MarkResourceOutlineAsOutdated();
            }
            catch (Exception e) { Debug.LogWarning("ResourceOutline failed to be created or updated: " + e.ToString()); }
            return AssetMoveResult.DidNotMove;
        }*/

        
    }
}
