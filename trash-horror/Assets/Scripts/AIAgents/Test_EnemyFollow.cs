using UnityEngine;
using UnityEngine.AI;

public class Test_EnemyFollow : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    public Transform goal;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position;
    }
}
