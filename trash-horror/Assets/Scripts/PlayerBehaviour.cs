using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour, ISerializable, IGameEventListener
{
	[Header("Calm Down Settings")]
	[Tooltip("If checked, Calm Down only works when near a 'Friendly' tagged object.")]
	[SerializeField] private bool requireFriendlyNearby = true;
	
	[Tooltip("The radius (in units) to detect friendly creatures.")]
	[SerializeField] private float friendlyCheckRadius = 5f;

	[Tooltip("The amount of sanity to gain when pressing the interaction button")] [SerializeField]
	private float sanityGainAmount = 0.1f;

	private Vector2 movement;
	private Rigidbody2D rb;

	public Camera camera;

	public InputActionAsset InputActions;

	private InputAction m_movement;
	public static PlayerBehaviour instance;
	private static readonly int IsHurt = Animator.StringToHash("IsHurt");
	private static readonly int LookingForward = Animator.StringToHash("LookingForward");
	private static readonly int IsMoving = Animator.StringToHash("IsMoving");


	private InputAction m_calmdown;

	[SerializeField]
	private float speedMultiplier = 80;

	private SanityController sanityController;

	private List<IInteractable> interactables = new List<IInteractable>();
	public FloatVariable healthData;
	public GameEvent onHealthChanged;
	
	
	// This tracks how many friendly objects we are near.
	private int friendlyCount = 0;
    
	// We are "near friendly" if the count is greater than 0
	private bool isNearFriendly => friendlyCount > 0;
    
    //Animation
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    //TODO: add hurt animation 

	private void OnEnable()
	{
		InputActions.FindActionMap("Player").Enable();
		onHealthChanged.RegisterListener(this);
	}

	private void OnDisable()
	{
		InputActions.FindActionMap("Player").Disable();
		onHealthChanged.UnregisterListener(this);
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
		_animator.SetBool(LookingForward, movement.y<=0);
		_animator.SetBool(IsMoving, (Math.Abs(movement.x)>0||Math.Abs(movement.y)>0));
		

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

		if (requireFriendlyNearby == false)
		{
			// --- MODE 1: "Always" ---
			SanityController.Instance.IncreaseSanity(sanityGainAmount);
			
		}
		else
		{
			// --- MODE 2: "Friendly Only" ---
			if (isNearFriendly)
			{
				SanityController.Instance.IncreaseSanity(sanityGainAmount);
			}

		}
	}
	
	private void FixedUpdate()
	{
		Movement();
		rb.AddForce(movement * speedMultiplier);
	}

	private void Update()
	{
		CalmDown();
		//if (rb.linearVelocity.magnitude > 2.0f)
		//{
			// Enable Audio Source
		//}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
		if (interactable != null)
		{
			interactables.Add(interactable);
		}
		
		// Check if the object that entered has the "Friendly" tag
		if (other.CompareTag("Friendly"))
		{
			// If yes, add one to our counter
			friendlyCount++;
			Debug.Log("Entered friendly aura. Count: " + friendlyCount);
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
		if (interactable != null)
		{
			interactables.Remove(interactable);
		}
		
		// Check if the object that left has the "Friendly" tag
		if (other.CompareTag("Friendly"))
		{
			// If yes, subtract one from our counter
			friendlyCount--;
			// (We use Mathf.Max to make sure it never goes below 0)
			friendlyCount = Mathf.Max(0, friendlyCount); 
			Debug.Log("Exited friendly aura. Count: " + friendlyCount);
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
		};
	}

	public void Deserialize(Dictionary<string, string> serialized)
	{
		transform.position = Vector3Serialization.Deserialize(serialized["pos"]);
	}

	private float _oldHealth = -1;
	public void OnEventRaised()
	{
		_animator.SetTrigger(IsHurt);
		_oldHealth = healthData.value;
	}
}
