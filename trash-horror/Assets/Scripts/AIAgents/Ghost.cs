using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ghost : Hostile
{
    protected override void Chasing()
    {
        base.Chasing();
        Vector2 direction = (Target.position - transform.position).normalized;
        Rigidbody.linearVelocity = direction * chaseSpeed;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    protected override void Patrol()
    {
        // 1. Check if we've arrived at our random destination
        float distance = Vector2.Distance(transform.position, TargetPatrolPosition);

        if (distance < waypointTolerance)
        {
            Rigidbody.linearVelocity = Vector2.zero;
            PatrolWaitTimer -= Time.fixedDeltaTime;

            if (PatrolWaitTimer <= 0)
            {
                // Wait is over: Get a new point and reset timer
                GenerateNewPatrolPoint();
                PatrolWaitTimer = patrolWaitTime;
            } 
        }
        else
        {
            // Haven't arrived yet. Keep moving
            Vector2 direction = (TargetPatrolPosition - (Vector2)transform.position).normalized;
            Rigidbody.linearVelocity = direction * patrolSpeed;
        }
    }

    protected override void GenerateNewPatrolPoint()
    {
        // Get a random direction and distance
        Vector2 randomDirection = Random.insideUnitCircle * patrolRadius;
        
        // Set the new target position relative to our spawn
        TargetPatrolPosition = SpawnPosition + randomDirection;
    }
}
