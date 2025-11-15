using System;
using UnityEngine;

public class Ghost : Hostile
{
    private Transform target;
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void FixedUpdate()
    {
        base.FixedUpdate();
        
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
}
