using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdentity : MonoBehaviour
{
    private static List<EnemyIdentity> enemies = new List<EnemyIdentity>();
    public int doesntmatter = 0;

    void Start()
    {
        enemies.Add(this);
        doesntmatter = 1;
    }

    private void OnDestroy()
    {
        enemies.Remove(this);
    }

    public static List<EnemyIdentity> GetEnemies() => enemies;
}
