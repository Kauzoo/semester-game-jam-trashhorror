using System;
using UnityEngine;

public class Ghost : Hostile
{
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        // Chase is specific to Ghost (can to through walls)
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }
}
