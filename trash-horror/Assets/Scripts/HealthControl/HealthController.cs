using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    // --- SINGLETON ---
    public static HealthController Instance { get; private set; }

    public FloatVariable healthData;
    public GameEvent onHealthChanged;
    public StringVariable levelNameData;
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
    
    [ContextMenu("Kill")]
    public void KillPlayer()
    {
        SetHealth(0);
    }
    
    private void SetHealth(float health)
    {
        healthData.value = health;
        onHealthChanged.Raise();
        
        if (healthData.value != 0) return;
        
        levelNameData.value = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Scenes/GameOver");
    }
}