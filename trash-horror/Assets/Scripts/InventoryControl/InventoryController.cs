using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class InventoryController : MonoBehaviour, ISerializable
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

    public bool HasKey()
    {
        return inventory.value > 0;
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

    public Dictionary<string, string> Serialize()
    {
        return new()
        {
            { "inventory", inventory.value.ToString(CultureInfo.CurrentCulture) }
        };
    }

    public void Deserialize(Dictionary<string, string> serialized)
    {
        inventory.value = int.Parse(serialized["inventory"], CultureInfo.InvariantCulture);
        onInventoryChanged.Raise();
    }
}
