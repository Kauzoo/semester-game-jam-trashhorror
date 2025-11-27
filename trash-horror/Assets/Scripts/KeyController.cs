using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour, IInteractable, IToggleable
{
    private bool _pickedUp;
    public AudioClip keyPickupAudioClip;
    public bool isVisible = true;

    private void Awake()
    {
        gameObject.SetActive(isVisible);
    }

    public void Interact(PlayerBehaviour player)
    {
        AudioSource.PlayClipAtPoint(keyPickupAudioClip, gameObject.transform.position);
        InventoryController.Instance.AddKey();
        gameObject.SetActive(false);
        _pickedUp = true;
    }

    public void On()
    {
        if (_pickedUp) return;

        gameObject.SetActive(true);
    }

    public void Off()
    {
        if (_pickedUp) return;

        gameObject.SetActive(false);
    }
}
