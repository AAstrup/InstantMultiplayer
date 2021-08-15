using InstantMultiplayer.Synchronization.Identification;
using Synchronization.Objects.Resources;
using System.Linq;
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

            EditorGUILayout.HelpBox(resourceOutline.Outdated ?
                $"{nameof(ResourceOutline)} is outdated. It will automatically be updated upon entering play mode or starting a build. You can also manually update it:"
                : $"{nameof(ResourceOutline)} is up-to-date."
                , MessageType.Info);

            GUILayout.BeginHorizontal();
            var buttonWidth = 100;
            GUILayout.Space(Screen.width / 2f - buttonWidth / 2f);
            if (GUILayout.Button(resourceOutline.Outdated ? "Update" : "Reupdate", GUILayout.Width(buttonWidth), GUILayout.Height(30)))
            {
                if(!resourceOutline.Outdated)
                {
                    var prevEntries = ResourceOutlineHelper.GetOrderedEntries();
                    ResourceOutlineHelper.UpdateResourceOutline();
                    var entries = ResourceOutlineHelper.GetOrderedEntries();
                    foreach(var entry in entries)
                    {
                        var prevEntry = prevEntries.FirstOrDefault(e =>
                            e.Name.Equals(entry.Name) && e.TypeName.Equals(entry.TypeName) && e.Path.Equals(entry.Path));
                        if(prevEntry != null && entry.Id != prevEntry.Id)
                        {
                            Debug.LogWarning($"Ids differ for entry [{entry}] and old entry [{prevEntry}]");
                        }
                    }
                }
                else
                    ResourceOutlineHelper.UpdateResourceOutline();
            }
            GUILayout.EndHorizontal();

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