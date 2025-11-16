using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeController : Instant
{
    protected override void Trigger(PlayerBehaviour player)
    {
        HealthController.Instance.DecreaseHealth(1);
        List<GameObject> res = new();
        GameObject.FindGameObjectsWithTag("AudioHandler", res);
        if (res[0] != null)
        {
            var audio_handler = res[0].GetComponent<AudioHandler>();
            audio_handler.PlayTrapDamage();
        }
    }
}
