using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "CustomEvent/Void")]
public class EventVoid : ScriptableObject
{
	private List<EventListenerVoid> listeners =
		new List<EventListenerVoid>();

	public void Raise()
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised();
	}

	public void RegisterListener(EventListenerVoid listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(EventListenerVoid listener)
	{ listeners.Remove(listener); }
}