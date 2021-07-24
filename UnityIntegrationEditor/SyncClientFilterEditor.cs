using InstantMultiplayer.UnityIntegration;
using UnityEditor;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegrationEditor
{
    [CustomEditor(typeof(SyncClientFilter), true)]
    public class SyncClientFilterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var comp = (SyncClientFilter)target;
            var oldFilterValue = comp.ClientFilter;
            for (int i = 0; i < 32; i++)
            {
                var bitRepr = 1 << i;
                var enabled = (comp.ClientFilter & bitRepr) != 0;
                var toggle = EditorGUILayout.Toggle("Client" + i.ToString(), enabled);
                if (toggle && !enabled)
                {
                    comp.ClientFilter |= bitRepr;
                }
                else if (!toggle && enabled)
                {
                    comp.ClientFilter &= ~bitRepr;
                }
            }

            var newFilterValue = EditorGUILayout.IntField("Filter value:", comp.ClientFilter);
            if (newFilterValue != comp.ClientFilter)
            {
                comp.ClientFilter = newFilterValue;
            }

            serializedObject.ApplyModifiedProperties();
            if (comp.ClientFilter != oldFilterValue)
            {
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }
    }
}