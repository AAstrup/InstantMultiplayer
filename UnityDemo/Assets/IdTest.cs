using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets
{
    public class IdTest: MonoBehaviour
    {
        public Object Object;

        private void Start()
        {
            Debug.Log(Object.GetInstanceID());
            Debug.Log(UnityObjectHelper.FindObjectFromInstanceID(Object.GetInstanceID()).name);
        }
    }
}
