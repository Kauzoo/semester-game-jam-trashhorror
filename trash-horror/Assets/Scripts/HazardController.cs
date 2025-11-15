using UnityEngine;
using Object = System.Object;

public abstract class HazardController : MonoBehaviour, IGameEventListener
{
    private SpriteRenderer _spriteRenderer;
    private Flasher _flasher;
    
    public GameEvent sanityEvent;
    public Sprite camouflagedSprite;
    public Sprite sprite;
    public float sanityThreshold = 0.4f;

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

        int sanity = 0;
        _spriteRenderer.sprite = sanity <= sanityThreshold ? camouflagedSprite : sprite;
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
