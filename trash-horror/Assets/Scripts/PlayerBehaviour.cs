using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour {

	private Vector2 movement;
	private Rigidbody2D rb;

	public Camera camera;

    public InputActionAsset InputActions;

    private InputAction m_movement;
	public static PlayerBehaviour instance;

	private InputAction m_interaction;

	[SerializeField]
	private float speedMultiplier = 80;
	
	private SanityController sanityController;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

	private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }


    private void Awake() {
		instance = this;
		rb = GetComponent<Rigidbody2D>();

		m_movement = InputSystem.actions.FindAction("Move");
		m_interaction = InputSystem.actions.FindAction("Interact");
	}

	private void Movement() {
        
		movement = m_movement.ReadValue<Vector2>();

		//Code to flip character to look left / right (Doesn work currently, needs adjusting if necessary)
		/*
		if (movement.x != 0 || movement.y != 0) {
			if (movement.x > 0.0) {
				transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
				camera.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

			} else if (movement.x < 0.0) {
				transform.localRotation = Quaternion.identity;
				camera.transform.localRotation = Quaternion.identity;
			}
		} else {
			// Idle Animation?
		}
		*/
		
	}

	// Interaction: Increase Sanity
	private void Interaction()
	{
		if (!m_interaction.WasPressedThisFrame()) return;
		
		Debug.Log("Interacted");
		SanityController.Instance.IncreaseSanity(0.1f);
	}

	private void FixedUpdate() {
		Movement();
		rb.AddForce(movement * speedMultiplier);
	}

	private void Update()
	{
		Interaction();
	}

}
