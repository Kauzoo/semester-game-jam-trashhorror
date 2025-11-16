using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IGameEventListener
{
    public FloatVariable health;
    public GameEvent onHealthChanged;

    public Image heart;
    public Sprite heartSprite;
    public Sprite emptyHeart;
    
    private readonly List<Image> _hearts = new();
    
    private void OnEnable()
    {
        onHealthChanged.RegisterListener(this);
        
        for (int i = 0; i < Mathf.Ceil(health.value); i++)
        {
            _hearts.Add(Instantiate(heart, gameObject.transform, false));
        }
        UpdateHearts();
    }

    private void OnDisable()
    {
        onHealthChanged.UnregisterListener(this);
    }

    [ContextMenu("Update Hearts")]
    public void RemoveHeart()
    {
        health.value -= 0.25f;
        onHealthChanged.Raise();
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
            heartImage.sprite = thisHeartsHealth == 0f ? emptyHeart : heartSprite;
            heartImage.fillAmount = thisHeartsHealth == 0f ? 1 : Mathf.Ceil(thisHeartsHealth * 4f) / 4f;
            currentHealth -= thisHeartsHealth;
        }
    }
} 