using UnityEngine;
using UnityEngine.Events;

public class EventListenerBase<T> : MonoBehaviour
{
    public EventBase<T> Event;
    public UnityEvent<T> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(T i)
    { Response.Invoke(i); }
}