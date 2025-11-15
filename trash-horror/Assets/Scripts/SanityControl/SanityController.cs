using UnityEngine;

public class SanityController : MonoBehaviour
{
    public FloatVariable sanityData; // Sanity is a value between 0.0 and 1.0
    public GameEvent onSanityChanged;
    
    // ---For Testing---
    [Header("Randomizer Settings")]
    [Tooltip("The minimum time (in seconds) between random sanity changes.")]
    [SerializeField] private float minChangeInterval = 2.0f;
    
    [Tooltip("The maximum time (in seconds) between random sanity changes.")]
    [SerializeField] private float maxChangeInterval = 8.0f;

    private float timer;
    
    // ------

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
    
    // // ---For Testing---
    private void Update()
    {
        // --- This is the logic you requested ---

        // 1. Count down the timer
        timer -= Time.deltaTime;

        // 2. When the timer runs out...
        if (timer <= 0f)
        {
            // 3. Calculate a new, completely random sanity value (between 0.0 and 1.0)
            float newSanityValue = Random.Range(0f, 1f);

            // 4. Update the global sanity data
            if (sanityData != null)
            {
                sanityData.value = newSanityValue;
                Debug.Log($"Sanity changed to: {newSanityValue}");
            }
            
            // 5. Raise the event to notify all listeners
            if (onSanityChanged != null)
            {
                onSanityChanged.Raise();
            }

            // 6. Reset the timer for the next random change
            ResetTimer();
        }
    }
    
    private void ResetTimer()
    {
        timer = Random.Range(minChangeInterval, maxChangeInterval);
    }
    
    // ------
    
    
}
