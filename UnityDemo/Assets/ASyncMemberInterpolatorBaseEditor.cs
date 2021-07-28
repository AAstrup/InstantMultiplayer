using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.Synchronization.Monitored;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(ASyncMemberInterpolatorBase), true)]
    public class Vector3InterpolatorEditor : Editor
    {
        private SerializedProperty _component;

        void OnEnable()
        {
            _component = serializedObject.FindProperty(nameof(ASyncMemberInterpolatorBase.Component));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_component);

            if (_component.objectReferenceValue != null)
            {
                /*var interpolator = (ASyncMemberInterpolatorBase)target;
                var generic = interpolator.GenericType;
                var comp = MonitorFactory.CreateComponentMonitor(0, interpolator.Component);
                //  .Where(m => m.GetValueTypeFromMemberInfo().IsAssignableFrom(generic));
                var memberNames = comp.Members.Select(m => m.).ToArray();
                interpolator.SelectedIndex = EditorGUILayout.Popup("Member", interpolator.SelectedIndex, memberNames);*/
            }

            serializedObject.ApplyModifiedProperties();
        } 
    }
}
