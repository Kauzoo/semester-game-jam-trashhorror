using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeController : Instant
{
    protected override void Trigger(PlayerBehaviour player)
    {
        HealthController.Instance.DecreaseHealth(1);
    }
}
