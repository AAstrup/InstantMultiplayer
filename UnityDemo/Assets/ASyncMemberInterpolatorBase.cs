using System;
using UnityEngine;

namespace Assets
{
    public abstract class ASyncMemberInterpolatorBase: MonoBehaviour
    {
        public Component Component;
        [HideInInspector]
        public int SelectedIndex;

        public int Timestamp { get; set; }
        internal abstract Type GenericType { get; }

        private void Start()
        {
            Debug.Log("awd");
        }
    }
}
