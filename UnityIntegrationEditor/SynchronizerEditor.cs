using InstantMultiplayer.UnityIntegration;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor
{
    [CustomEditor(typeof(Synchronizer), true)]
    public class SynchronizerEditor : Editor
    {
        private SerializedProperty _clientFilter;
        private SerializedProperty _behaviours;
        private HashSet<int> _expandedComponents;

        void OnEnable()
        {
            _clientFilter = serializedObject.FindProperty(nameof(Synchronizer.ClientFilter));
            _behaviours = serializedObject.FindProperty(nameof(Synchronizer.Components));
            _expandedComponents = new HashSet<int>();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (targets.Length > 1)
            {
                EditorGUILayout.HelpBox("Multiselect not supported", MessageType.Warning);
                return;
            }

            EditorGUILayout.PropertyField(_clientFilter);

            var synchronizer = (Synchronizer)target;
            if (Application.isPlaying && synchronizer.Initialized)
            {
                EditorGUILayout.LabelField("Id:", synchronizer.SynchronizerId.ToString());
                EditorGUILayout.LabelField("Owner:", synchronizer.OwnerId.ToString());
                //_expandComponents = EditorGUILayout.BeginFoldoutHeaderGroup(_expandComponents, "Components");
                EditorGUILayout.LabelField("Components:");
                foreach (var component in synchronizer.ComponentMonitors)
                {
                    var id = component.MonitoredInstance.GetInstanceID();
                    var expanded = _expandedComponents.Contains(id);
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    var shouldExpand = EditorGUILayout.BeginFoldoutHeaderGroup(expanded, component.MonitoredInstance?.ToString() ?? "null");
                    if (expanded)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        EditorGUILayout.BeginVertical();
                        foreach (var member in component.Members)
                        {
                            EditorGUILayout.LabelField($"{member.Name} [{member.MemberType}]:", member.LastValue?.ToString() ?? "null");
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();

                    if(!expanded && shouldExpand)
                    {
                        _expandedComponents.Add(id);
                    }
                    else if(expanded && !shouldExpand)
                    {
                        _expandedComponents.Remove(id);
                    }
                }
                //EditorGUILayout.EndFoldoutHeaderGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(_behaviours);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
