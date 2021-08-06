using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace InstantMultiplayer.UnityIntegrationEditor.ResourceOutlines
{
    internal class PreProcessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            ResourceOutlineHelper.UpdateResourceOutline();
        }
    }
}
