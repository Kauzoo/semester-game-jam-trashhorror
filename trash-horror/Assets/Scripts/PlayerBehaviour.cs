using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour, ISerializable
{

	private Vector2 movement;
	private Rigidbody2D rb;

	public Camera camera;

	public InputActionAsset InputActions;

	private InputAction m_movement;
	public static PlayerBehaviour instance;
	

	private InputAction m_calmdown;

	[SerializeField]
	private float speedMultiplier = 80;

	private SanityController sanityController;

	private List<String> inventory = new List<String>();
	private List<IInteractable> interactables = new List<IInteractable>();
    
    //Animation
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    //TODO: add hurt animation 

	private void OnEnable()
	{
		InputActions.FindActionMap("Player").Enable();
	}

	private void OnDisable()
	{
		InputActions.FindActionMap("Player").Disable();
	}


	private void Awake()
	{
		instance = this;
		rb = GetComponent<Rigidbody2D>();

		m_movement = InputSystem.actions.FindAction("Move");
		m_calmdown = InputSystem.actions.FindAction("Calm down");
		InputSystem.actions.FindAction("Interact").started += OnInteract;
		
		// animation
		_animator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

	}


	private void Movement()
	{

		movement = m_movement.ReadValue<Vector2>();

		//flip sprite 
		if (movement.x < 0)
		{
			_spriteRenderer.flipX=true;
		}
		else
		{
			_spriteRenderer.flipX=false;
		}
		
		//animation trigger
		_animator.SetBool("LookingForward",movement.y<=0);
		_animator.SetBool("IsMoving",(Math.Abs(movement.x)>0||Math.Abs(movement.y)>0));
		

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

	private void CalmDown()
	{
		if (!m_calmdown.WasPressedThisFrame()) return;

		Debug.Log("[" + string.Join(", ", inventory.ToArray()) + "]");
		Debug.Log("Calmed down");
		SanityController.Instance.IncreaseSanity(0.1f);
	}

	private void FixedUpdate()
	{
		Movement();
		rb.AddForce(movement * speedMultiplier);
	}

	private void Update()
	{
		CalmDown();
	}

	public void AddToInventory(string item)
	{
		inventory.Add(item);
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
		if (interactable != null)
		{
			interactables.Add(interactable);
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
		if (interactable != null)
		{
			interactables.Remove(interactable);
		}
	}

	private void OnInteract(InputAction.CallbackContext context)
	{
		interactables.FirstOrDefault()?.Interact(this);
	}

	public Dictionary<string, string> Serialize()
	{
		return new()
		{
			{"pos", transform.position.Serialize()},
			{"inv", string.Join(",", inventory.ToArray())},
		};
	}

	public void Deserialize(Dictionary<string, string> serialized)
	{
		transform.position = Vector3Serialization.Deserialize(serialized["pos"]);
		string inv = serialized["inv"];
		inventory = string.IsNullOrWhiteSpace(inv) ? new List<string>() : inv.Split(',').ToList();
	}
}	
