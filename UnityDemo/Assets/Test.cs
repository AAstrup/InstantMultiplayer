using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private bool _value;
    private bool _printToggle;

    private void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            _printToggle = !_printToggle;
        if (_printToggle)
            Debug.Log("TEST");
    }
}
