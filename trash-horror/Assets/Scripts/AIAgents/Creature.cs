using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public abstract class Creature : MonoBehaviour, IGameEventListener
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Camouflage = Animator.StringToHash("Camouflage");

    [Header("CreatureSettings")]
    [Tooltip("How fast the creature moves during chase")]
    public float chaseSpeed = 3.0f;

    [Tooltip("How fast the creature moves during chase")]
    public float patrolSpeed = 1.0f;

    [Tooltip("The range at which the creature will detect the player.")]
    public float detectionRadius = 8f;

    [Tooltip("The radius around its spawn point that the creature will wander in.")]
    public float patrolRadius = 10f;

    [Tooltip("The time the creature waits until it searches a new patrol point")]
    public float patrolWaitTime = 3f;

    [Tooltip("How close the creature needs to be to a point to consider it 'arrived'.")]
    public float waypointTolerance = 1.0f;

    [Tooltip("How often the hostile can deal damage (in seconds).")]
    public float effectInterval = 1.0f;  // Cooldown between hits

    public FloatVariable sanity;
    public GameEvent onSanityChanged;
    public float sanityThreshold = 0.8f;
    public bool aboveThreshold;

    protected UnityEngine.AI.NavMeshAgent Agent;
    protected Transform Target;
    protected Rigidbody2D Rigidbody;
    protected Collider2D BodyCollider;

    protected float EffectTimer;            // Timer for the effect cooldown
    protected Vector2 SpawnPosition;        // The "home" position to patrol around
    protected Vector2 TargetPatrolPosition; // The current random point we are moving to
    protected float PatrolWaitTimer;        // Timer for waiting at a point

    private SpriteRenderer _renderer;
    private Animator _animator;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Rigidbody = GetComponent<Rigidbody2D>();
        BodyCollider = GetComponent<BoxCollider2D>();

        // Find the trigger collider and set its radius
        GetComponent<CircleCollider2D>().radius = detectionRadius;

        if (Agent)
        {
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
        }

        // Initialize Patrolling
        SpawnPosition = transform.position; // Remember where we started
        PatrolWaitTimer = patrolWaitTime; // Set the wait timer
        GenerateNewPatrolPoint();  // Get our first random point
    }

    private void OnEnable()
    {
        onSanityChanged.RegisterListener(this);
    }

    private void OnDisable()
    {
        onSanityChanged.UnregisterListener(this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // If the player enters our detection range, set them as the target
        if (!other.gameObject.CompareTag("Player")) return;

        // 1. Calculate the direction and distance to the player
        Vector2 direction = other.transform.position - transform.position;
        float distance = direction.magnitude;

        // 2. Cast a ray FROM us, TO the player, checking ONLY for walls
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            distance,
            LayerMask.GetMask("Wall")
        );

        // 3. Check the result
        // The ray hit NOTHING: This means we have a clear line of sight. We are clear to chase.
        // The ray HIT A WALL : We do NOT have line of sight.
        Target = hit.collider == null ? other.transform : null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player leaves our detection range, lose them as a target.
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player lost! Returning to idle.");
        Target = null;
    }

    protected void FixedUpdate()
    {
        if (EffectTimer > 0)
        {
            EffectTimer -= Time.fixedDeltaTime;
        }
        else
        {
            BodyCollider.enabled = true;
        }

        if (Target)
        {
            Chasing();
        }
        else
        {
            Patrol();
        }
    }

    protected virtual void Chasing()
    {
        PatrolWaitTimer = 0;

        // If Chasing look at target
        Vector3 direction = Target.position - transform.position;

        _animator.SetBool(Running, Mathf.Abs(direction.x) > 0 || Mathf.Abs(direction.y) > 0);

        _renderer.flipX = direction.x > 0;

        if (!Agent) return;

        Agent.speed = chaseSpeed;
        Agent.destination = Target.position;
        _animator.SetFloat(Speed, Agent.speed);
    }

    protected abstract void Patrol();

    // Generates a new random patrol point within the patrolRadius.
    protected abstract void GenerateNewPatrolPoint();

    public void OnEventRaised()
    {
        _animator.SetBool(Camouflage, sanity.value >= sanityThreshold == aboveThreshold);
    }
}
