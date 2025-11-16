using UnityEngine;

public class SanityController : MonoBehaviour
{
    // --- SINGLETON ---
    public static SanityController Instance { get; private set; }
    
    
    public FloatVariable sanityData; // Sanity is a value between 0.0 and 1.0
    public GameEvent onSanityChanged;
    
    [Header("Sanity Decrease Settings")]
    [Tooltip("The amount of sanity that decreases very timer")]
    [SerializeField] private float decreaseSanityAmount = 0.1f;
    
    [Tooltip("The amount of seconds for each decrease of sanity")]
    [SerializeField] private float sanityDecreaseInterval = 1.0f;

    // Is the Decrease of sanity slowed down? Friendly AIAgents can do this
    private bool lowerSanityDecrease = false;
    public float LowerSanityDecreasePercentage = 0.75f;

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
        if (lowerSanityDecrease)
        {
            amount = amount * LowerSanityDecreasePercentage;
        }

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

    public void SetLowerSanityDecrease(bool lowerSanityDecrease)
    {
        this.lowerSanityDecrease = lowerSanityDecrease;
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
    
    // Decrease Sanity by amount
    private void Update()
    {
        // 1. Count down the timer
        timer -= Time.deltaTime;

        // 2. When the timer runs out...
        if (timer <= 0f)
        {
            // If Sanity is 0, the player loses health
            if (sanityData.value <= 0)
            {
                HealthController.Instance.DecreaseHealth(0.25f);
            }
            else
            {
                // 3. Calculate a new sanity value
                //float newSanityValue = Random.Range(0f, 0.25f);
                float newSanityValue = decreaseSanityAmount;

                // 4. Update the global sanity data
                if (sanityData != null)
                {
                    DecreaseSanity(newSanityValue);
                    Debug.Log($"Sanity changed to: {sanityData.value}");
                }
            }
            // 6. Reset the timer for the next random change
            timer += sanityDecreaseInterval;
        }
    }
}
