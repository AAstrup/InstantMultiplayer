using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.UnityIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public class SyncTest : MonoBehaviour
{
    public Text Text;
    public Synchronizer Synchronizer;

    private DeltaContainer _delta;

    private void Start()
    {
        Text.text = ComponentMapper.GetCIDFromType(typeof(Transform)).ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            _delta = Synchronizer.TryGetDeltaContainer(new DeltaProvider(), out var delta) ? delta : null;
        if (Input.GetKeyDown(KeyCode.L))
            Synchronizer.ConsumeDeltaContainer(new DeltaConsumer(), _delta);
    }
}
