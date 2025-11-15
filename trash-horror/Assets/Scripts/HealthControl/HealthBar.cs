using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IGameEventListener
{
    public FloatVariable health;
    public GameEvent onHealthChanged;

    public Image heart;
    public float maxHealth;
    
    private readonly List<Image> _hearts = new();
    
    private void OnEnable()
    {
        health.value = maxHealth;
        onHealthChanged.RegisterListener(this);
        
        for (int i = 0; i < Mathf.Ceil(maxHealth); i++)
        {
            _hearts.Add(Instantiate(heart, gameObject.transform, false));
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
            heartImage.fillAmount = thisHeartsHealth;
            currentHealth -= thisHeartsHealth;
        }
    }
}