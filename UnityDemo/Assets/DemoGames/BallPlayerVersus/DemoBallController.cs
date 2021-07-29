using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBallController : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField]
    private float multiplier = 2f;
    [SerializeField]
    private float boost = 10f;
    private float lastBoostTime;
    [SerializeField]
    private float boostCoolDown = 5f;

    void Start()
    {
        body = GetComponent<Rigidbody>();    
    }

    void FixedUpdate()
    {
        body.AddForce(multiplier * new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")));
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && lastBoostTime + boostCoolDown < Time.time)
        {
            lastBoostTime = Time.time;
            body.AddForce(boost * new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")), ForceMode.VelocityChange);
        }
    }
}
