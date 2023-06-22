using UnityEngine;
using System.Collections.Generic;

// [CreateAssetMenu(menuName = "CustomEvent/Bool")]
public class EventBase<T> : ScriptableObject
{
	private List<EventListenerBase<T>> listeners =
		new List<EventListenerBase<T>>();

	public void Raise(T j)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(j);
	}

	public void RegisterListener(EventListenerBase<T> listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(EventListenerBase<T> listener)
	{ listeners.Remove(listener); }
}