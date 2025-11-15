using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour, IInteractable
{
    public void Interact(PlayerBehaviour player)
    {
        player.AddToInventory("Key");
        gameObject.SetActive(false);
    }

    public Dictionary<string, string> Serialize()
    {
        return new()
        {
            { "active", gameObject.activeSelf.ToString() },
        };
    }

    public void Deserialize(Dictionary<string, string> serialized)
    {
        gameObject.SetActive(bool.Parse(serialized["active"]));
    }
}
