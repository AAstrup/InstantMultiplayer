using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.UnityIntegration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Test : MonoBehaviour
    {
        public Text Text;
        public List<Object> Objects;

        private void Start()
        {
            Objects.Add(Resources.GetBuiltinResource<Mesh>("Cube.fbx"));
            Text.text = "";
            foreach (var obj in Objects)
                Text.text += obj.name + " " + obj.GetType() + " " + obj.GetInstanceID() + "\n";
        }
     
        void Update()
        {
        }
    }
}