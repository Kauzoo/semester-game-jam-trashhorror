using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ghost : Hostile
{
    
    
    
    protected override void Chasing()
    {
        base.Chasing();
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    protected override void Patrol()
    {
        // 1. Check if we've arrived at our random destination
        float distance = Vector2.Distance(transform.position, targetPatrolPosition);

        if (distance < waypointTolerance)
        {
            rb.linearVelocity = Vector2.zero;
            patrolWaitTimer -= Time.fixedDeltaTime;

            if (patrolWaitTimer <= 0)
            {
                // Wait is over: Get a new point and reset timer
                GenerateNewPatrolPoint();
                patrolWaitTimer = patrolWaitTime;
            } 
        }
        else
        {
            // Haven't arrived yet. Keep moving
            Vector2 direction = (targetPatrolPosition - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * patrolSpeed;
        }
    }

    protected override void GenerateNewPatrolPoint()
    {
        // Get a random direction and distance
        Vector2 randomDirection = Random.insideUnitCircle * patrolRadius;
        
        // Set the new target position relative to our spawn
        targetPatrolPosition = spawnPosition + randomDirection;
    }
}
