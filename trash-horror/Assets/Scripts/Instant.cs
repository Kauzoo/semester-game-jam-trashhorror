using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public abstract class Instant : MonoBehaviour, IGameEventListener
{
    private static readonly int Camouflage = Animator.StringToHash("Camouflage");
    private Animator _animator;
    private Flasher _flasher;
    private AudioSource _audioSource;
    
    public FloatVariable sanity;
    public GameEvent sanityEvent;
    public float sanityThreshold = 0.8f;
    public bool camouflageAboveThreshold;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _flasher = GetComponent<Flasher>();
        _audioSource = GetComponent<AudioSource>();
        sanityEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        sanityEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        if (_flasher.isFlashing) return;
        
        _animator.SetBool(Camouflage, sanity.value >= sanityThreshold == camouflageAboveThreshold);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _animator.SetBool(Camouflage, false);

        _audioSource.Play(0);

        _flasher.StartFlashing(() => gameObject.SetActive(false));

        PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
        Trigger(player);
    }

    protected abstract void Trigger(PlayerBehaviour player);
}
