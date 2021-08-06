using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [Serializable]
    [CreateAssetMenu(fileName = "demo", menuName = "Demo", order = 100)]
    public class ScriptableObjectTest: ScriptableObject
    {
        public List<int> Values;
    }
}
