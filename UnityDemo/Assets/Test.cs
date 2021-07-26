using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.UnityIntegration;
using Synchronization.Extensions;
using Synchronization.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Test : MonoBehaviour
    {
        public Text Text;
        public List<Object> Objects;
        public Synchronizer Synchronizer;
        public MeshFilter MeshFilter;

        private void Start()
        {
            Objects.Add(Resources.GetBuiltinResource<Mesh>("Cube.fbx"));
            Text.text = Application.dataPath + "\n";
            foreach (var obj in Objects)
                Text.text += obj.name + " " + obj.GetType() + " " + obj.GetInstanceID() + "\n";
        }
     
        void Update()
        {
            if (SynchronizeStore.Instance.LocalId != 0)
            {
                Text.text = "ClientId: " + SynchronizeStore.Instance.LocalId.ToString() + "\n"
                    + "SynchronizerId: " + Synchronizer.SynchronizerId + "\n" +
                    MeshFilter.sharedMesh.name + "\n" +
                    UnityObjectHelper.FindObjectFromInstanceID(MeshFilter.sharedMesh.GetInstanceID()).name;
            }
        }
    }
}