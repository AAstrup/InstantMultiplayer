using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
    }
}
