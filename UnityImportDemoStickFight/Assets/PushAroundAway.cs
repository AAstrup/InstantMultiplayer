using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAroundAway : MonoBehaviour
{
    [SerializeField]
    private float distance = 1f;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            foreach (var enemy in EnemyIdentity.GetEnemies())
            {
                var delta = enemy.transform.position - transform.position;
                if(delta.magnitude < distance)
                {
                    enemy.transform.position += delta.normalized * distance;
                }
            }
    }
}
