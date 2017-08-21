using System;

/// <summary>
/// Wraps a variable in a class that triggers an
/// event if the value changes. This is useful when
/// values can be meaningfully compared using Equals,
/// and when the variable changes infrequently in
/// comparison to the number of times it is updated.
/// </summary>
/// <typeparam name="T">The type of the value you want to observe</typeparam>
/// <remarks>This is a typical use case:
/// <code>
/// ObservedValue&lt;bool&gt; showWindow;
/// 
/// public void Start()
/// {
///		show = new ObservedValue(false);
///     show.OnValueChanged += ShowHideWindow;
/// }
/// public void OnGUI()
/// {
///		showWindow.Value = GUILayout.Toggle("Show Window", showWindow.Value);
/// }
/// 
/// public void ShowHideWindow()
/// {
///		window.gameObject.SetActive(showWindow.Value);
/// }
/// </code>
/// </remarks>

public class ObservedValue<T> {
	T currentValue;

	/// <summary>
	/// Subscribe to this event to get notified when the value changes.
	/// </summary>
	public event Action OnValueChange;

	public ObservedValue(T initialValue) {
		currentValue = initialValue;
	}

	public T Value {
		get { return currentValue; }

		set {
			if (!currentValue.Equals(value)) {
				currentValue = value;

				if (OnValueChange != null) {
					OnValueChange();
				}
			}
		}
	}

	/// <summary>
	/// Sets the value without notification.
	/// </summary>
	/// <param name="value">The value.</param>
	public void SetSilently(T value) {
		currentValue = value;
	}
}
