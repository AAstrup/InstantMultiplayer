using UnityEditor;

namespace InstantMultiplayer.UnityIntegration
{
    [CustomEditor(typeof(Synchronizer), true)]
    public class SynchronizerEditor : Editor
    {
        private SerializedProperty _clientFilter;
        private SerializedProperty _behaviours;

        void OnEnable()
        {
            _clientFilter = serializedObject.FindProperty(nameof(Synchronizer.ClientFilter));
            _behaviours = serializedObject.FindProperty(nameof(Synchronizer.Components));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_clientFilter);
            EditorGUILayout.PropertyField(_behaviours);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
