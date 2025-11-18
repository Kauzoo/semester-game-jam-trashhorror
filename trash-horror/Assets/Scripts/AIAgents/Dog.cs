using UnityEngine;
using UnityEngine.AI;

public class Dog : Friendly
{
    protected override void Patrol()
    {
        Agent.speed = patrolSpeed;
        
        // 1. Check if we've arrived at our random destination
        if (!Agent.pathPending && Agent.remainingDistance < waypointTolerance)
        {
            PatrolWaitTimer -= Time.fixedDeltaTime;

            if (!(PatrolWaitTimer <= 0)) return;
            
            // Wait is over: Get a new point and reset timer
            GenerateNewPatrolPoint();
            PatrolWaitTimer = patrolWaitTime;
        } 
        else if (!Agent.pathPending)
        {
            PatrolWaitTimer -= Time.fixedDeltaTime;
        }
    }
    
    protected override void GenerateNewPatrolPoint()
    {
        // We will try a few times to find a valid point, in case we get unlucky
        for (int i = 0; i < 10; i++)
        {
            // 1. Get a random point (same as before)
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += new Vector3(SpawnPosition.x, SpawnPosition.y, 0); 

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas)) continue;
            
            // 2. We found a valid point on the mesh.
            //    NOW, check if we can actually *reach* it.
            NavMeshPath path = new NavMeshPath();
            if (!Agent.CalculatePath(hit.position, path)) continue;
                
            // 3. Check the path status
            // If path.status is PathPartial or PathInvalid, the point
            // is on an unreachable island. We do nothing and let the loop try again.
            if (path.status != NavMeshPathStatus.PathComplete) continue;
                
            // 4. SUCCESS! The path is complete and reachable.
            //    Set this as our destination and exit the function.
            TargetPatrolPosition = hit.position;
            Agent.destination = TargetPatrolPosition;
            return;
        }
    }
}
