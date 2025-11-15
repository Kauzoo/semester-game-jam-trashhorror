using UnityEngine;
using UnityEngine.AI;

public class Hunter : Hostile
{
    private UnityEngine.AI.NavMeshAgent agent;
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Chasing()
    {
        base.Chasing();
        agent.destination = target.position;
    }

    protected override void Patrol()
    {
        // 1. Check if we've arrived at our random destination
        float distance = Vector2.Distance(transform.position, targetPatrolPosition);

        if (distance < waypointTolerance)
        {
            patrolWaitTime -= Time.fixedDeltaTime;

            if (patrolWaitTime <= 0)
            {
                // Wait is over: Get a new point and reset timer
                GenerateNewPatrolPoint();
                patrolWaitTimer = patrolWaitTime;
            } 
        }
        else
        {
            agent.destination = targetPatrolPosition;
        }
    }
    
    protected virtual void GenerateNewPatrolPoint()
    {
        // 1. Get a random point *in a circle* (as a 3D vector).
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += new Vector3(spawnPosition.x,spawnPosition.y,0) ; // Add it to our "home" position

        // 2. Find the *nearest valid point on the NavMesh* to our random point.
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            // 3. We found a valid point! Set it as the new destination.
            Debug.Log(hit.position);
            agent.destination = hit.position;
            targetPatrolPosition = hit.position;
        }
        // If it fails, it just won't set a new destination and will try again
        // next time it finishes waiting.
    }
}

