using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Hostile : Creature
{
    [Header("Hostile Enemy Settings")] [Tooltip("How fast the hostile moves during chase")]
    public float chaseSpeed = 3.0f;

    [Tooltip("How fast the hostile moves during chase")]
    public float patrolSpeed = 1.0f;
    

    [Header("Hostile Combat")] 
    [Tooltip("How much damage the hostile deals on contact")]
    public float touchDamage = 0.1f;
    [Tooltip("How much sanity loss the hostile deals on contact")]
    public float sanityLoss = 0.1f;

    [Tooltip("How often the hostile can deal damage (in seconds).")]
    public float damageInterval = 1.0f;  // Cooldown between hits
    
    [Tooltip("The range at which the hostile will detect the player.")]
    public float detectionRadius = 8f;
    [Tooltip("The radius around its spawn point that the hostile will wander in.")]
    public float patrolRadius = 10f;
    
    public float patrolWaitTime = 3f;
    [Tooltip("How close the ghost needs to be to a point to consider it 'arrived'.")]
    public float waypointTolerance = 1.0f;
    
    private float damageTimer; // Timer for the damage cooldown
    protected Transform target;
    protected Rigidbody2D rb;
    
    protected Vector2 spawnPosition;        // The "home" position to patrol around
    protected Vector2 targetPatrolPosition; // The current random point we are moving to
    protected float patrolWaitTimer;        // Timer for waiting at a point
    
    public Collider2D bodyCollider;
    
    protected override void Start()
    {
        base.Start();
        
        rb = GetComponent<Rigidbody2D>();
            
        // Find the trigger collider and set its radius
        foreach (var col in GetComponents<CircleCollider2D>())
        {
            if (col.isTrigger)
            {
                col.radius = detectionRadius;
                break; // Assumes only one trigger collider
            }
        }
        
        // Initialize Patrolling
        spawnPosition = transform.position; // Remember where we started
        GenerateNewPatrolPoint();  // Get our first random point
        patrolWaitTimer = patrolWaitTime; // Set the wait timer

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        // 1. Check if the thing we hit is the "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // 2. Check if damage cooldown has finished
            if (damageTimer <= 0)
            {
                Debug.Log($"Ghost hit {collision.gameObject.name}!");
                
                // 3. Deal the damage + sanity loss
                HealthController.Instance.DecreaseHealth(touchDamage);
                SanityController.Instance.DecreaseSanity(sanityLoss);
                
                // 4. Reset cooldown timer
                damageTimer = damageInterval;
                
                bodyCollider.enabled = false;
                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // If the player enters our detection range, set them as the target
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player detected! Starting chase.");
            
            /*var hits = Physics2D.BoxCastAll(transform.position, new Vector2(5,5), 0, transform.up, detectionRadius);
            foreach (var hit in hits)
            {
                if (hit.transform == other.transform)
                {
                    target = other.transform;
                    break;
                }
            }*/
            
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
            if (hit.collider == null)
            {
                // The ray hit NOTHING. This means we have a clear line of sight.
                // We are clear to chase.
                target = other.transform;
            }
            else
            {
                // The ray HIT A WALL before it reached the player.
                // We do NOT have line of sight.
                target = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player leaves our detection range, lose them as a target.
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player lost! Returning to idle.");
            target = null;
        }
    }

    protected void FixedUpdate()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.fixedDeltaTime;
        }
        else
        {
            bodyCollider.enabled = true;
        }

        if (target != null)
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
        patrolWaitTimer = 0;
        
        // If Chasing look at target
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    protected virtual void Patrol()
    {
    }

    // Generates a new random patrol point within the patrolRadius.
    protected virtual void GenerateNewPatrolPoint()
    {
    }
}
