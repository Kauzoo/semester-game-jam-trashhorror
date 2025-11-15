using UnityEngine;
using UnityEngine.Events;

// This component implements the interface, so the GameEvent can talk to it.
// This is the general structure that an event listener needs to have!
public class ExampleGameEventListener : MonoBehaviour, IGameEventListener
{
    [Tooltip("The event channel to listen to.")]
    public GameEvent gameEvent;

    [Tooltip("The functions to run when the event is raised.")]
    public UnityEvent response;

    // When the object is enabled, it registers itself with the event.
    private void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(this);
        }
    }

    // When the object is disabled, it unregisters itself.
    private void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
        }
    }

    // This is the function required by the IGameEventListener interface.
    // The GameEvent will call this function, which in turn
    // triggers the public 'response' event you see in the inspector.
    public void OnEventRaised()
    {
        response.Invoke();
    }
}
