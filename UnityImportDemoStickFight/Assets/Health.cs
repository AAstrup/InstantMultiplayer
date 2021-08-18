using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    public void Damage(int v)
    {
        health -= v;
        if(health <= 0)
            Destroy(gameObject);
    }
}