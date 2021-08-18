using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterWeapon : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mPos = Input.mousePosition;
            mPos.z = transform.position.z;
            var worldMPos = Camera.main.ScreenToWorldPoint(mPos);
            var diff = worldMPos - transform.position;
            diff.z = 0;
            var hit = Physics2D.Raycast((Vector2)transform.position, diff, 1000f, layerMask);
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                var target = hit.collider.GetComponent<Health>();
                target.Damage(1);
            }
        }
    }
}
