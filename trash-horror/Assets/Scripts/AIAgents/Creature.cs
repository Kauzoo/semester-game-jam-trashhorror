using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public abstract class Creature : MonoBehaviour, IEntity
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
       // Destroy(gameObject);
       gameObject.SetActive(false);
    }

    public Dictionary<string, string> Serialize()
    {
        return new()
        {
            {"pos", gameObject.transform.position.Serialize() },
            {"maxHealth", maxHealth.ToString(CultureInfo.CurrentCulture) },
            {"currentHealth", currentHealth.ToString(CultureInfo.CurrentCulture) },
        };
    }

    public void Deserialize(Dictionary<string, string> serialized)
    {
        gameObject.GetComponent<Rigidbody2D>().position = Vector3Serialization.Deserialize(serialized["pos"]);
        maxHealth = float.Parse(serialized["maxHealth"]);
        currentHealth = float.Parse(serialized["currentHealth"]);
        gameObject.SetActive(currentHealth > 0);
    }
}
