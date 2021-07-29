using InstantMultiplayer.Synchronization.Delta;
using System;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation
{
    public abstract class ASyncMemberInterpolatorBase : MonoBehaviour
    {
        public Component Component;
        [HideInInspector]
        public int SelectedIndex;

        public abstract Type GenericType { get; }

        abstract internal void DeltaConsumeHandler(DeltaMember deltaMember);
    }
}
