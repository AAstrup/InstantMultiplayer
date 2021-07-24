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
        public List<UnityEngine.Object> Objects;

        private void Start()
        {
            foreach (var obj in Objects)
                Text.text += obj.GetInstanceID() + "\n";
        }
     
        void Update()
        {
        }
    }
}