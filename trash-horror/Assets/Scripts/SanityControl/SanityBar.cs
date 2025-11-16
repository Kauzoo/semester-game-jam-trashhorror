using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour, IGameEventListener
{
    [SerializeField] public FloatVariable sanity;
    [SerializeField] public GameEvent onSanityChange;
    [SerializeField] Image fullSlider;

    private void Start()
    {
        fullSlider.fillAmount = sanity.value;
    }
    
    // When the object is enabled, it registers itself with the event.
    private void OnEnable()
    {
        if (onSanityChange != null)
        {
            onSanityChange.RegisterListener(this);
        }
    }

    // When the object is disabled, it unregisters itself.
    private void OnDisable()
    {
        if (onSanityChange != null)
        {
            onSanityChange.UnregisterListener(this);
        }
    }
    
    public void OnEventRaised()
    {
        fullSlider.fillAmount = sanity.value;
    }
}
