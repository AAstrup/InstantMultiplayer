using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoFollowRelativeDistance : MonoBehaviour
{
    public Transform followTarget;
    private Vector3 relativeDistance;

    void Start()
    {
        relativeDistance = transform.position - followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.position + relativeDistance;
    }
}
