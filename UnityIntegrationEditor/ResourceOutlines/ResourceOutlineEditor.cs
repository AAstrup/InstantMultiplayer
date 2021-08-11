using InstantMultiplayer.Synchronization.Identification;
using Synchronization.Objects.Resources;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor.ResourceOutlines
{
    [CustomEditor(typeof(ResourceOutline), true)]
    public class ResourceOutlineEditor : Editor
    {
        private SerializedProperty _entries;

        void OnEnable()
        {
            _entries = serializedObject.FindProperty(nameof(ResourceOutline.Entries));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var resourceOutline = (ResourceOutline)target;
            if (resourceOutline.Outdated)
            {
                EditorGUILayout.HelpBox($"{nameof(ResourceOutline)} is outdated. It will automatically be updated upon entering play mode or starting a build. You can also manually update it:"
                    , MessageType.Info);

                GUILayout.BeginHorizontal();
                var buttonWidth = 100;
                GUILayout.Space(Screen.width / 2f - buttonWidth / 2f);
                if (GUILayout.Button("Update", GUILayout.Width(buttonWidth), GUILayout.Height(30)))
                {
                    ResourceOutlineHelper.UpdateResourceOutline();
                }
                GUILayout.EndHorizontal();
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(_entries);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("Types supported by IIdProvider implementations:");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            foreach (var registerdType in IdFactory.Instance.RegisteredTypes())
            {
                EditorGUILayout.LabelField(registerdType.FullName);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}