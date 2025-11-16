using UnityEngine;
using UnityEngine.UI;

public class InventoryBarController : MonoBehaviour, IGameEventListener
{
    public IntVariable inventory;
    public Image key;
    public GameEvent onInventoryChanged;

    private int currentInventory = 0;

    private void OnEnable()
    {
        onInventoryChanged.RegisterListener(this);
    }

    private void OnDisable()
    {
        onInventoryChanged.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        if (currentInventory < inventory.value)
        {
            for (int i = currentInventory; i < inventory.value; i++)
            {
                Instantiate(key, gameObject.transform, false);
            }
        }
        else
        {
            for (int i = currentInventory - 1; i >= inventory.value; i--)
            {
                Destroy(gameObject.transform.GetChild(i));
            }
        }
        currentInventory = inventory.value;
    }
}
