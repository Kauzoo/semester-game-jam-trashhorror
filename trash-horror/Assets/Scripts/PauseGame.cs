using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public InputActionAsset InputActions;
    public GameObject PauseCanvas;
    private InputAction pause_action;

    public bool IsPaused = false;
    public Button m_ResumeButton;

    private void OnEnable()
    {
        InputActions.FindActionMap("Other").Enable();
    }

	private void OnDisable()
    {
        InputActions.FindActionMap("Other").Disable();
    }

    private void Awake()
    {
		pause_action = InputSystem.actions.FindAction("Pause");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_ResumeButton.onClick.AddListener(PauseApplication);
    }

    // Update is called once per frame
    void Update()
    {
        ListenPauseGameKey();
    }
    
    

    private void ListenPauseGameKey()
    {
        if (pause_action.WasPressedThisFrame())
        {
            PauseApplication();
        }
    }

    private void PauseApplication()
    {
        if (!IsPaused)
        {
            Time.timeScale = 0;
            IsPaused = true;
            PauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            IsPaused = false;
            PauseCanvas.SetActive(false);
        }
    }
}
