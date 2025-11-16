using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // --- SINGLETON ---
    public static InventoryController Instance { get; private set; }

    public IntVariable inventory;
    public GameEvent onInventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inventory.value = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Increase")]
    public void AddKey()
    {
        inventory.value++;
        onInventoryChanged.Raise();
    }

    [ContextMenu("Decrease")]
    public void RemoveKey()
    {
        inventory.value--;
        onInventoryChanged.Raise();
    }
}
