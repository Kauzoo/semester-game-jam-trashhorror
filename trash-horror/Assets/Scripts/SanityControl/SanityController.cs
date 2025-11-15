using UnityEngine;

public class SanityController : MonoBehaviour
{
    public FloatVariable sanityData; // Sanity is a value between 0.0 and 1.0
    public GameEvent onSanityChanged;

    public void DecreaseSanity(float amount)
    {
        sanityData.value -= amount;
        onSanityChanged.Raise();
    }

    public void IncreaseSanity(float amount)
    {
        sanityData.value += amount;
        onSanityChanged.Raise();
    }
}
