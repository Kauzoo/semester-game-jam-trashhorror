using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour, IInteractable, IToggleable
{
    private bool _pickedUp;
    public bool isVisible = true;

    private void OnEnable()
    {
        gameObject.SetActive(isVisible);
    }

    public void Interact(PlayerBehaviour player)
    {
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
        if (_pickedUp) return;

        gameObject.SetActive(true);
    }

    public void Off()
    {
        if (_pickedUp) return;

        gameObject.SetActive(false);
    }
}
