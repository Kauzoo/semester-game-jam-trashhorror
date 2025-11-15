using UnityEngine;

public interface IInteractable : IEntity
{
    public void Interact(PlayerBehaviour player);
}
