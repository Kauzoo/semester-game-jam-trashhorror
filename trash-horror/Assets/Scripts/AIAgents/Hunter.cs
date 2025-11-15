using UnityEngine;

public class Hunter : Hostile
{
    private UnityEngine.AI.NavMeshAgent agent;
    
    void Start()
    {
        base.Start();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        if (target != null)
        {
            agent.destination = target.position;
        }
    }
}
