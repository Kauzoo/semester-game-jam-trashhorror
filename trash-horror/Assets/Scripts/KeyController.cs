using UnityEngine;

public class KeyController : MonoBehaviour, IInteractable
{
    public void Interact(PlayerBehaviour player)
    {
        player.AddToInventory("Key");
        Destroy(this.gameObject);
    }
}
