using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IGameEventListener
{
    public FloatVariable health;
    public GameEvent onHealthChanged;

    public GameObject heart;
    
    private readonly List<Image> _hearts = new();
    
    private void Start()
    {
        onHealthChanged.RegisterListener(this);
        
        for (int i = 0; i < Mathf.Ceil(health.value); i++)
        {
            GameObject obj = Instantiate(heart, gameObject.transform, false);
            Image img = obj.GetComponentsInChildren<Image>().Last();
            _hearts.Add(img);
        }
        UpdateHearts();
    }

    private void OnDisable()
    {
        onHealthChanged.UnregisterListener(this);
    }
    
    public void OnEventRaised()
    {
        UpdateHearts();
    }
    
    private void UpdateHearts()
    {
        float currentHealth = health.value;
        
        foreach (Image heartImage in _hearts)
        {
            float thisHeartsHealth = Mathf.Min(currentHealth, 1f);
            heartImage.fillAmount = Mathf.Ceil(thisHeartsHealth * 4f) / 4f;
            currentHealth -= thisHeartsHealth;
        }
    }
} 