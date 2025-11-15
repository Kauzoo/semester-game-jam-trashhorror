using UnityEditor;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [Header("Creature Stats")]
    public float maxHealth;
    public float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= Mathf.Clamp(amount, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public virtual void Heal(float amount)
    {
        currentHealth += Mathf.Clamp(amount, 0, maxHealth);
    }

    protected virtual void Die()
    {
       Destroy(gameObject);
    }
}
