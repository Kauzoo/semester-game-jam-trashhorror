using System;
using UnityEngine;

public class GateController : MonoBehaviour, IToggleable
{
    public Sprite closedSprite;
    public Sprite openedSprite;
    
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void On()
    {
        _spriteRenderer.sprite = openedSprite;
        _boxCollider2D.enabled = false;
    }

    public void Off()
    {
        _spriteRenderer.sprite = closedSprite;
        _boxCollider2D.enabled = true;
    }
}
