using UnityEngine;

public class LeverController : InteractableDevice
{
    public override void Interact(PlayerBehaviour player)
    {
        ChangeState(!isOn);
    }
}
