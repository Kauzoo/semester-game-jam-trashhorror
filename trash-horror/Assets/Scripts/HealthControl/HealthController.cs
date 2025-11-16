
using UnityEngine;

public class HealthController : MonoBehaviour
{
    // --- SINGLETON ---
    public static HealthController Instance { get; private set; }

    public FloatVariable healthData;
    public GameEvent onHealthChanged;
    public float maxHealth = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            healthData.value = maxHealth;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DecreaseHealth(float amount)
    {
        SetHealth(Mathf.Max(0, healthData.value - amount));
    }

    public void IncreaseHealth(float amount)
    {
        SetHealth(Mathf.Min(maxHealth, healthData.value + amount));
    }

    private void SetHealth(float health)
    {
        if (!(Mathf.Abs(health - healthData.value) < 0.001)) return;
        
        healthData.value = health;
        onHealthChanged.Raise();
    }
}