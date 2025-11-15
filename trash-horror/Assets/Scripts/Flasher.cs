using System;
using System.Collections.Generic;
using UnityEngine;

public class Flasher : MonoBehaviour
{
    public bool isFlashing;
    public int flashCount = 4;
    public float flashingSpeed = 10;

    private int _flashDirection = -1;
    private int _flashCounter;
    private SpriteRenderer _spriteRenderer;

    private Action _callback;

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isFlashing) return;

        if (_spriteRenderer.color.a <= 0)
        {
            _flashDirection = 1;
            _flashCounter++;
            if (flashCount == _flashCounter)
            {
                isFlashing = false;
                _callback?.Invoke();
            }
        }
        else if (_spriteRenderer.color.a >= 1)
        {
            _flashDirection = -1;
        }

        float alpha = _spriteRenderer.color.a + flashingSpeed * _flashDirection * Time.deltaTime;
        _spriteRenderer.color = new Color(1, 1, 1, alpha);
    }

    public void StartFlashing(Action callback = null)
    {
        _callback = callback;
        isFlashing = true;
        _flashCounter = 0;
    }
}