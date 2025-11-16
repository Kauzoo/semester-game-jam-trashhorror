using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Hostile : Creature
{

    [Header("Hostile Combat")] 
    [Tooltip("How much damage the hostile deals on contact")]
    public float touchDamage = 0.1f;
    [Tooltip("How much sanity loss the hostile deals on contact")]
    public float sanityLoss = 0.1f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        // 1. Check if the thing we hit is the "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // 2. Check if damage cooldown has finished
            if (effectTimer <= 0)
            {
                Debug.Log($"Hostile hit {collision.gameObject.name}!");
                
                // 3. Deal the damage + sanity loss
                HealthController.Instance.DecreaseHealth(touchDamage);
                SanityController.Instance.DecreaseSanity(sanityLoss);
                
                // 4. Reset cooldown timer
                effectTimer = effectInterval;
                
                bodyCollider.enabled = false;
                
                // TODO Maybe add knockback here
                
            }
        }
    }
}
