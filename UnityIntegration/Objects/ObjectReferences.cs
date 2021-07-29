using InstantMultiplayer.Synchronization.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Objects
{
    public class ObjectReferences: MonoBehaviour
    {
        public List<Object> Objects;

        public void Awake()
        {
            ReferenceRepository.Instance.Commit(Objects);
        }
    }
}
