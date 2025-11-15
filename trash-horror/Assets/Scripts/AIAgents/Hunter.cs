using Mono.Cecil;
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
        // We will try a few times to find a valid point, in case we get unlucky
        for (int i = 0; i < 10; i++)
        {
            // 1. Get a random point (same as before)
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += new Vector3(spawnPosition.x, spawnPosition.y, 0); 

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            {
                // 2. We found a valid point on the mesh.
                //    NOW, check if we can actually *reach* it.
                
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(agent.transform.position, hit.position, NavMesh.AllAreas, path))
                {
                    // 3. Check the path status
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        // 4. SUCCESS! The path is complete and reachable.
                        //    Set this as our destination and exit the function.
                        agent.SetDestination(hit.position);
                        return;
                    }
                    // If path.status is PathPartial or PathInvalid, the point
                    // is on an unreachable island. We do nothing and let the loop try again.
                }
            }
        }

        // If we failed 10 times, just stay put.
    }
}

