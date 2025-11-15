using UnityEngine;

public class SanityController : MonoBehaviour
{
    // --- SINGLETON ---
    public static SanityController Instance { get; private set; }
    
    
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
    
    private void Awake()
    {
        // This is the core singleton logic.
        if (Instance == null)
        {
            // If I am the first 'Instance', I am the one and only.
            Instance = this;
            
            // Optional: Don't destroy this object when loading new scenes
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // If an 'Instance' already exists, then I am a duplicate and must be destroyed.
            Destroy(gameObject);
        }
    }

    public void DecreaseSanity(float amount)
    {
        if (sanityData.value - amount <= 0)
        {
           sanityData.value = 0;
           onSanityChanged.Raise();
        }
        else
        {
            sanityData.value -= amount;
            onSanityChanged.Raise();
        }
    }

    public void IncreaseSanity(float amount)
    {
        if (sanityData.value + amount >= 1)
        {
            sanityData.value = 1;
            onSanityChanged.Raise();
        }
        else
        {
            sanityData.value += amount;
            onSanityChanged.Raise();
        }
    }
    
    // // ---For Testing---
    // Decrease Sanity by a random amount
    private void Update()
    {
        // 1. Count down the timer
        timer -= Time.deltaTime;

        // 2. When the timer runs out...
        if (timer <= 0f)
        {
            // 3. Calculate a new, completely random sanity value (between 0.0 and 1.0)
            float newSanityValue = Random.Range(0f, 0.25f);

            // 4. Update the global sanity data
            if (sanityData != null)
            {
                DecreaseSanity(newSanityValue);
                Debug.Log($"Sanity changed to: {sanityData.value}");
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
