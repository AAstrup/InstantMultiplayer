using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.UnityIntegration.Interpolation;
using System;
using System.Linq;
using UnityEditor;

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

            if (_component.objectReferenceValue != null)
            {
                var interpolator = (ASyncMemberInterpolatorBase)target;
                var generic = interpolator.GenericType;
                var comp = MonitorFactory.CreateComponentMonitor(0, interpolator.Component);
                var memberNames = comp.Members
                    .Where(m => m.MemberType.IsAssignableFrom(generic))
                    .Select(m => m.Name)
                    .ToArray();
                var prevTargetMemberName = comp.Members[_index.intValue].Name;
                var prevTypedMemberIndex = Array.IndexOf(memberNames, prevTargetMemberName);
                var newTypedMemberIndex = EditorGUILayout.Popup("Member", prevTypedMemberIndex, memberNames);
                var newTargetMemberName = memberNames[newTypedMemberIndex];
                for(int i=0; i<comp.Members.Length; i++)
                {
                    if(comp.Members[i].Name == newTargetMemberName)
                    {
                        _index.intValue = i;
                        break;
                    }
                }
            }

            EditorGUILayout.PropertyField(_localLerping);
            EditorGUILayout.PropertyField(_localLerpScale);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
