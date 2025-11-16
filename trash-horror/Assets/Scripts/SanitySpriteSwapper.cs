 using UnityEngine;

// This script requires a SpriteRenderer to function.
[RequireComponent(typeof(SpriteRenderer))]
public class SanitySpriteSwapper : MonoBehaviour, IGameEventListener
{
    [Header("Sanity References")]
    [Tooltip("The FloatVariable holding the current sanity value.")]
    [SerializeField] private FloatVariable sanityData;

    [Tooltip("The GameEvent that fires when sanity changes.")]
    [SerializeField] private GameEvent onSanityChanged;

    [Header("Sprite Settings")]
    [Tooltip("The sprite to show when sanity is HIGH (camouflaged).")]
    [SerializeField] private Sprite camouflagedSprite;

    [Tooltip("The sprite to show when sanity is LOW (normal).")]
    [SerializeField] private Sprite normalSprite;

    [Tooltip("The sanity value (0-1) to switch sprites at. " +
             "If sanity is >= this, it shows the camouflaged sprite.")]
    [SerializeField] private float sanityThreshold = 0.8f;

    private SpriteRenderer spriteRenderer;

    // When the component is first enabled (or game starts)
    private void OnEnable()
    {
        // Get the SpriteRenderer on this same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SanitySpriteSwapper requires a SpriteRenderer component.", this);
            return;
        }

        // Register with the event to listen for changes
        if (onSanityChanged != null)
        {
            onSanityChanged.RegisterListener(this);
        }

        // Immediately check and set the correct sprite on start
        UpdateSprite();
    }

    // When the component is disabled
    private void OnDisable()
    {
        // Stop listening to the event to prevent errors
        if (onSanityChanged != null)
        {
            onSanityChanged.UnregisterListener(this);
        }
    }

    /// <summary>
    /// This is the function called by the 'sanityEvent' (from IGameEventListener).
    /// </summary>
    public void OnEventRaised()
    {
        // When the event fires, update our sprite
        UpdateSprite();
    }

    /// <summary>
    /// Checks the sanity value and sets the correct sprite.
    /// </summary>
    private void UpdateSprite()
    {
        // Make sure we have all the references we need
        if (sanityData == null || spriteRenderer == null)
        {
            return;
        }

        // This is the core logic:
        // Check if sanity value is high
        bool isSane = (sanityData.value >= sanityThreshold);

        // Set the sprite based on the check.
        // Use the camouflagedSprite if sane, otherwise use the normalSprite.
        spriteRenderer.sprite = isSane ? camouflagedSprite : normalSprite;
    }
}