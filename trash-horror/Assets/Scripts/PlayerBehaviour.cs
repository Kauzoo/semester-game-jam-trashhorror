using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour, IGameEventListener
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

	public AudioSource steps;

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
		
		// Animation
		_animator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}


	private void Movement()
	{

		movement = m_movement.ReadValue<Vector2>();

		//flip sprite 
		_spriteRenderer.flipX = movement.x < 0;
		
		//animation trigger
		_animator.SetBool(LookingForward, movement.y <= 0);
		_animator.SetBool(IsMoving, Math.Abs(movement.x) > 0 || Math.Abs(movement.y) > 0);
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

	public void AddFriendlySource()
	{
		friendlyCount++;
	}

	public void RemoveFriendlySource()
	{
		friendlyCount = Mathf.Max(0, friendlyCount - 1);
	}

	private void FixedUpdate()
	{
		Movement();
		rb.AddForce(movement * speedMultiplier);
	}

	private void Update()
	{
		CalmDown();
		if (rb.linearVelocity.magnitude > 3.0f)
		{
			steps.enabled = true;
		}
		else
		{
			steps.enabled = false;
		}
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
	
	private float _oldHealth = -1;
	public void OnEventRaised()
	{
		_animator.SetTrigger(IsHurt);
		_oldHealth = healthData.value;
	}
}
