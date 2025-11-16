using UnityEngine;

/// <summary>
/// This script goes on a child GameObject of the Player.
/// Its only job is to manage the large "friendly detection" trigger
/// and report its findings to the main PlayerBehaviour script.
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class FriendlyDetector : MonoBehaviour
{
    [Tooltip("The radius (in units) to detect friendly objects.")] [SerializeField]
    private float friendlyCheckRadius = 5f;

    // A reference to the main script on our parent
    private PlayerBehaviour playerBehaviour;

    private void Awake()
    {
        // Get the main player script from our parent object
        playerBehaviour = GetComponentInParent<PlayerBehaviour>();
        if (playerBehaviour == null)
        {
            Debug.LogError("FriendlyDetector cannot find PlayerBehaviour script on parent!");
        }

        // Setup the collider
        var collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = friendlyCheckRadius;
    }

    /// <summary>
    /// Called by Unity's 2D physics when a collider ENTERS our large trigger.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered has the "Friendly" tag
        if (other.CompareTag("Friendly"))
        {
            // Tell the main player script we are near a friendly
            playerBehaviour.AddFriendlySource();
        }
    }

    /// <summary>
    /// Called by Unity's 2D physics when a collider EXITS our large trigger.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that left has the "Friendly" tag
        if (other.CompareTag("Friendly"))
        {
            // Tell the main player script we left a friendly
            playerBehaviour.RemoveFriendlySource();
        }
    }
}
