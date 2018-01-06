using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIButton : MonoBehaviour {

	[SerializeField] bool isEnabledAtStart = true;
	[SerializeField] UnityEvent onClickValidated;

	TextMeshProUGUI text;
	SelectableUI selectableUI;
	bool isEnabled = true;
	public bool IsEnabled { get { return isEnabled; } }

	void Start() {
		text = transform.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
		selectableUI = GetComponent<SelectableUI>();

		if (!isEnabledAtStart) {
			Disable();
		}
	}

	public void Enable() {
		Color color = text.color;
		Color newColor = new Color(color.r, color.g, color.b, 1f);

		text.color = newColor;
		isEnabled = true;
		selectableUI.IsDisabled = false;
	}

	public void Disable() {
		Color color = text.color;
		Color newColor = new Color(color.r, color.g, color.b, 0.5f);

		text.color = newColor;
		isEnabled = false;
		selectableUI.IsDisabled = true;
	}

	public void OnClick() {
		if (isEnabled) {
			onClickValidated.Invoke();
		}
	}

}
