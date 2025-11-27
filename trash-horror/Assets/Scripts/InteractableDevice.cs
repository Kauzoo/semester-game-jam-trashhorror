using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableDevice : MonoBehaviour, IInteractable
{
    public GameObject toggledGameObject;
    public Sprite onSprite;
    public Sprite offSprite;

    private IToggleable toggled;

    protected bool isOn = false;

    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        toggled = toggledGameObject?.GetComponent<IToggleable>();
        audioSource = GetComponent<AudioSource>();

        if (toggled == null)
        {
            Debug.LogError($"{name}: Assigned object does not implement IToggleable.", this);
        }
    }

    public void ChangeState(bool newOn)
    {
        if (newOn)
        {
            isOn = true;
            toggled?.On();
        }
        else
        {
            isOn = false;
            toggled?.Off();
        }

        audioSource.Play(0);
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        spriteRenderer.sprite = isOn ? onSprite : offSprite;
    }

    public abstract void Interact(PlayerBehaviour player);
}
