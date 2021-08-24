using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    void Update()
    {
        bool anyFound = false;
        Vector3 closestDelta = Vector3.zero;
        foreach (var player in PlayerIdentity.GetPlayers())
        {
            var delta = player.transform.position - transform.position;
            if(!anyFound || delta.magnitude < closestDelta.magnitude)
            {
                closestDelta = delta;
            }
        }
        transform.position += closestDelta.normalized * moveSpeed * Time.deltaTime;
    }
}
