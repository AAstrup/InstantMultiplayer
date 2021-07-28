using System;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public abstract class ASyncMemberInterpolatorBase: MonoBehaviour
    {
        public Component Component;
        public int Timestamp { get; set; }
        internal abstract Type GenericType { get; }
        internal int SelectedIndex;
    }
}
