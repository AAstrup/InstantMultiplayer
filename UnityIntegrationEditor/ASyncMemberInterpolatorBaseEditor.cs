using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.UnityIntegration.Interpolation;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor
{
    [CustomEditor(typeof(ASyncMemberInterpolatorBase), true)]
    public class Vector3InterpolatorEditor : Editor
    {
        private SerializedProperty _component;
        private SerializedProperty _index;
        private SerializedProperty _localLerping;
        private SerializedProperty _localLerpScale;

        void OnEnable()
        {
            _component = serializedObject.FindProperty(nameof(ASyncMemberInterpolatorBase.Component));
            _index = serializedObject.FindProperty(nameof(ASyncMemberInterpolatorBase.SelectedIndex));
            _localLerping = serializedObject.FindProperty(nameof(ASyncMemberInterpolatorBase.LocalLerping));
            _localLerpScale = serializedObject.FindProperty(nameof(ASyncMemberInterpolatorBase.LocalLerpScale));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_component);

            var interpolator = (ASyncMemberInterpolatorBase)target;
            var generic = interpolator.GenericType;
            if (interpolator.Component != null)
            {
                var comp = MonitorFactory.CreateComponentMonitor(0, interpolator.Component);
                var memberNames = comp?.Members
                    .Where(m => m.MemberType.IsAssignableFrom(generic))
                    .Select(m => m.Name)
                    .ToArray();
                if (memberNames != null)
                {
                    var prevTargetMemberName = comp.Members[_index.intValue].Name;
                    var prevTypedMemberIndex = Array.IndexOf(memberNames, prevTargetMemberName);
                    var newTypedMemberIndex = EditorGUILayout.Popup("Member", prevTypedMemberIndex, memberNames);
                    var newTargetMemberName = memberNames[newTypedMemberIndex];
                    for (int i = 0; i < comp.Members.Count; i++)
                    {
                        if (comp.Members[i].Name == newTargetMemberName)
                        {
                            _index.intValue = i;
                            break;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Failed to infer component or component member monitors.");
                }

                EditorGUILayout.PropertyField(_localLerping);
                if (_localLerping.boolValue)
                {
                    EditorGUILayout.PropertyField(_localLerpScale);
                }
            }
            else
            {
                EditorGUILayout.HelpBox($"Add a component with a {generic} member", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
