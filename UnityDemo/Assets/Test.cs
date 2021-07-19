using System;
using UnityEngine;
using UnityIntegration;

public class Test : MonoBehaviour
{
    [SerializeField]
    private bool _value;
    private bool _printToggle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            _printToggle = !_printToggle;
        if (_printToggle)
            Debug.Log("TEST");
    }
}
