using System;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public abstract class Instant : MonoBehaviour, IGameEventListener
{
    private SpriteRenderer _spriteRenderer;
    protected Flasher _flasher;

    public FloatVariable sanity;
    public GameEvent sanityEvent;
    public Sprite camouflagedSprite;
    public Sprite sprite;
    public float sanityThreshold = 0.8f;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

        _spriteRenderer.sprite = sanity.value >= sanityThreshold ? camouflagedSprite : sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _spriteRenderer.sprite = sprite;

        _flasher.StartFlashing(() => Destroy(gameObject));

        PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
        Trigger(player);
    }

    protected abstract void Trigger(PlayerBehaviour player);
}
