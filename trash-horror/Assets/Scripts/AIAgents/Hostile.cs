using UnityEngine;

public class Hostile : Creature
{
    private UnityEngine.AI.NavMeshAgent agent;
    public Transform goal;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        agent.destination = goal.position;
    }
}
