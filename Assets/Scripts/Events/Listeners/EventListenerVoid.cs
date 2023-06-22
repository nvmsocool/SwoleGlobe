using UnityEngine;
using UnityEngine.Events;

public class EventListenerVoid : MonoBehaviour
{
    public EventVoid Event;
    public UnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    { Response.Invoke(); }
}