using System;

public class SpikeController : Instant
{
    protected override void Trigger(PlayerBehaviour player)
    {
        HealthController.Instance.DecreaseHealth(1);
    }
}
