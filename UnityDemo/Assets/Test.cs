using InstantMultiplayer.UnityIntegration;
using System.Collections.Generic;
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
            if (SyncClient.Instance.LocalId != 0)
            {
                Text.text = "ClientId: " + SyncClient.Instance.LocalId.ToString() + "\n"
                    + "SynchronizerId: " + Synchronizer.SynchronizerId + "\n" +
                    MeshFilter.sharedMesh.name + "\n" +
                    UnityObjectHelper.FindObjectFromInstanceID(MeshFilter.sharedMesh.GetInstanceID()).name;
            }
        }
    }
}