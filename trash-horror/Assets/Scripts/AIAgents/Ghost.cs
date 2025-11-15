using System;
using UnityEngine;

public class Ghost : Hostile
{
    private Transform target;
    private Rigidbody2D rb;
    public float speed = 2.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
