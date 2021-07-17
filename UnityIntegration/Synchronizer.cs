using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIntegration
{
    public class Synchronizer: MonoBehaviour
    {
        public SyncClientFilter ClientFilter;
        public List<MonoBehaviour> Behaviours;
    }
}
