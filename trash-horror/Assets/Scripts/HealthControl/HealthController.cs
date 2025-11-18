using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour, IGameEventListener
{
    // --- SINGLETON ---
    public static HealthController Instance { get; private set; }

    public HealthVariable healthData;
    public GameEvent onHealthChanged;
    public GameEvent onRespawn;
    public StringVariable levelNameData;
    public float maxHealth = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            healthData.max = maxHealth;
            healthData.current = maxHealth;
            onRespawn.RegisterListener(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DecreaseHealth(float amount)
    {
        SetHealth(Mathf.Max(0, healthData.current - amount));
    }

    public void IncreaseHealth(float amount)
    {
        SetHealth(Mathf.Min(maxHealth, healthData.current + amount));
    }
    
    [ContextMenu("Kill")]
    public void KillPlayer()
    {
        SetHealth(0);
    }
    
    private void SetHealth(float health)
    {
        healthData.current = health;
        onHealthChanged.Raise();
        
        if (healthData.current != 0) return;
        
        levelNameData.value = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Scenes/GameOver");
    }

    public void OnEventRaised()
    {
        healthData.current = maxHealth;
    }
}