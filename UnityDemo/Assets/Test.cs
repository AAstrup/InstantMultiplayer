using UnityEngine;
using UnityIntegration;

public class Test : MonoBehaviour
{
    public bool Value;
    private bool _printToggle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            _printToggle = !_printToggle;
        if (_printToggle)
            Debug.Log("TEST");
    }
}
