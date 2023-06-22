using UnityEngine.InputSystem;

public class EventBaseExtant<T> : EventBase<T> where T : struct
{
	public void RaiseFromContext(InputAction.CallbackContext value)
	{
		// check button?
		Raise(value.ReadValue<T>());
	}
}