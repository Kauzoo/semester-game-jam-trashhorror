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

    public Dictionary<string, string> Serialize()
    {
        return new()
        {
            { "active", gameObject.activeSelf.ToString() },
            { "pickedUp", _pickedUp.ToString() }
        };
    }

    public void Deserialize(Dictionary<string, string> serialized)
    {
        gameObject.SetActive(bool.Parse(serialized["active"]));
        _pickedUp = bool.Parse(serialized["pickedUp"]);
    }

    public void On()
    {
        Debug.Log("On");
        if (_pickedUp) return;
        Debug.Log("On2");

        gameObject.SetActive(true);
    }

    public void Off()
    {
        if (_pickedUp) return;

        gameObject.SetActive(false);
    }
}
