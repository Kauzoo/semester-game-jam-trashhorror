
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour, ISerializable
{
    // --- SINGLETON ---
    public static HealthController Instance { get; private set; }

    public FloatVariable healthData;
    public GameEvent onHealthChanged;
    public GameEvent onRespawn;
    public float maxHealth = 3;

    private bool _dead;

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

    private void OnEnable()
    {
        healthData.value = maxHealth;
        onHealthChanged.Raise();
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
        if (_dead) return;
        
        healthData.value = health;
        onHealthChanged.Raise();
        
        if (healthData.value != 0) return;
        
        _dead = true;
        StartCoroutine(nameof(Die));
    }

    private IEnumerator Die()
    {
        SceneManager.LoadScene("Scenes/GameOver", LoadSceneMode.Additive);
        yield return new WaitForSeconds(5);
        yield return SceneManager.UnloadSceneAsync("Scenes/GameOver");
        onRespawn.Raise(); 
        _dead = false;
    }

    public Dictionary<string, string> Serialize()
    {
        return new()
        {
            { "health", healthData.value.ToString(CultureInfo.CurrentCulture) }
        };
    }

    public void Deserialize(Dictionary<string, string> serialized)
    {
        healthData.value = float.Parse(serialized["health"], CultureInfo.InvariantCulture);
        onHealthChanged.Raise();
    }
}