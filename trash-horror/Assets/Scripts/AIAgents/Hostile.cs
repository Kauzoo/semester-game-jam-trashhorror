using System;
using UnityEngine;

public class Hostile : Creature
{
    [Header("Hostile Enemy Settings")] [Tooltip("How fast the hostile moves")]
    public float speed = 2.0f;

    [Header("Hostile Combat")] [Tooltip("How much damage the hostile deals on contact")]
    public float touchDamage = 0.1f;
    [Tooltip("How much sanity loss the hostile deals on contact")]
    public float sanityLoss = 0.1f;

    [Tooltip("How often the hostile can deal damage (in seconds).")]
    public float damageInterval = 1.0f;  // Cooldown between hits
    
    private float damageTimer; // Timer for the damage cooldown

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
            }
        }
    }

    protected void FixedUpdate()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.fixedDeltaTime;
        }
    }
}
