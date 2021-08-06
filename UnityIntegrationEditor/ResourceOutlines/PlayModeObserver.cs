using UnityEditor;

namespace InstantMultiplayer.UnityIntegrationEditor.ResourceOutlines
{
    [InitializeOnLoadAttribute]
    public static class PlayModeStateChangedExample
    {
        static PlayModeStateChangedExample()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
                ResourceOutlineHelper.UpdateResourceOutline();
        }
    }
}
