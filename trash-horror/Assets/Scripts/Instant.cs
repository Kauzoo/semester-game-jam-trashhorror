using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public abstract class Instant : MonoBehaviour, IGameEventListener
{
    private Animator _animator;
    protected Flasher _flasher;

    public string actualTrigger;
    public string camouflageTrigger;
    
    public FloatVariable sanity;
    public GameEvent sanityEvent;
    public float sanityThreshold = 0.8f;
    public bool aboveThreshold;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _flasher = GetComponent<Flasher>();
        sanityEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        sanityEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        if (_flasher.isFlashing) return;
        
        _animator.SetTrigger(sanity.value >= sanityThreshold == aboveThreshold ? camouflageTrigger : actualTrigger);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _animator.SetTrigger(actualTrigger);

        _flasher.StartFlashing(() => gameObject.SetActive(false));

        PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
        Trigger(player);
    }

    protected abstract void Trigger(PlayerBehaviour player);
}
