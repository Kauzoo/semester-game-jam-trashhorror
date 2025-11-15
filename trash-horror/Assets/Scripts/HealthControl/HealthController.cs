
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public FloatVariable healthData;
    public GameEvent onHealthChanged;
    
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