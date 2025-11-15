using System;
using UnityEngine;
using Object = System.Object;

public abstract class HazardController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Flasher _flasher;
    
    public Sprite camouflagedSprite;
    public Sprite sprite;
    public int sanityThreshold = 40;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flasher = GetComponent<Flasher>();
    }

    private void Update()
    {
        if (_flasher.isFlashing) return;
        
        int sanity = 0; // TODO Set from Game Controller
        if (sanity >= sanityThreshold)
        {
            _spriteRenderer.sprite = camouflagedSprite;
        }
        else
        {
            _spriteRenderer.sprite = sprite;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _spriteRenderer.sprite = sprite;
        
        _flasher.StartFlashing(() => Destroy(gameObject));
        
        // TODO Deal damage
        // PlayerController player = other.gameObject.GetComponent<PlayerController>();
        // TriggerTrap(player);
    }

    protected abstract void TriggerTrap(/*PlayerController*/Object player);
}
