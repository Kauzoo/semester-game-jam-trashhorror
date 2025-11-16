using UnityEngine;
using UnityEngine.AI;

public class Guard : Hostile
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
}

