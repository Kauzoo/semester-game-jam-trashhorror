using UnityEngine;
using UnityEngine.AI;

public class Guard : Hostile
{
    private UnityEngine.AI.NavMeshAgent agent;

    private Animator _animator;
    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        base.Start();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }

    protected override void Chasing()
    {
        _animator.SetFloat("Speed",agent.speed);
        base.Chasing();
        agent.destination = target.position;
    }
}

