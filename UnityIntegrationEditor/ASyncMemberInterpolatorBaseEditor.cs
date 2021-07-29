using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.UnityIntegration.Interpolation;
using System.Linq;
using UnityEditor;

namespace InstantMultiplayer.UnityIntegrationEditor
{
    [CustomEditor(typeof(ASyncMemberInterpolatorBase), true)]
    public class Vector3InterpolatorEditor : Editor
    {
        private SerializedProperty _component;
        private SerializedProperty _index;

        void OnEnable()
        {
            _component = serializedObject.FindProperty(nameof(ASyncMemberInterpolatorBase.Component));
            _index = serializedObject.FindProperty(nameof(ASyncMemberInterpolatorBase.SelectedIndex));
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
                var prevIndex = interpolator.SelectedIndex;
                _index.intValue = EditorGUILayout.Popup("Member", _index.intValue, memberNames);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
