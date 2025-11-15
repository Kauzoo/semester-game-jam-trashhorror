
using UnityEngine;

public class HealthController : MonoBehaviour
{
    // --- SINGLETON ---
    public static HealthController Instance { get; private set; }

    public FloatVariable healthData;
    public GameEvent onHealthChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DecreaseHealth(float amount)
    {
        healthData.value -= amount;
        onHealthChanged.Raise();
    }

    public void IncreaseHealth(float amount)
    {
        healthData.value += amount;
        onHealthChanged.Raise();
    }
}