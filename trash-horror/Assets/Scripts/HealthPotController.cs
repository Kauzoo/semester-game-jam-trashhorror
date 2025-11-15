using UnityEngine;

public class HealthPotController : Instant
{
    protected override void Trigger(PlayerBehaviour player)
    {
        HealthController.Instance.IncreaseHealth(1);
    }
}
