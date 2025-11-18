using UnityEngine;

public abstract class Friendly : Creature
{
    /*
     [Tooltip("How much heal the friendly deals on contact")]
    public float touchHeal = 0.1f;
    [Tooltip("How much sanity gain the friendly deals on contact")]
    public float sanityGain = 0.1f;
    */

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 1. Check if the thing we hit is the "Player"
        if (!collision.gameObject.CompareTag("Player")) return;
        
        // 2. Check if damage cooldown has finished
        if (!(EffectTimer <= 0)) return;
        
        Debug.Log($"Friendly hit {collision.gameObject.name}!");
                
        // 3. Lower the Sanity Decrease when Friendly is around
        SanityController.Instance.SetLowerSanityDecrease(true);
                
        // 4. Reset cooldown timer
        EffectTimer = effectInterval;
                
        BodyCollider.enabled = false;
    }
}

