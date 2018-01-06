using UnityEngine;
using TMPro;

public class UIButton : MonoBehaviour {

	[SerializeField] bool isEnabledAtStart = true;

	TextMeshProUGUI text;
	bool isEnabled = true;
	public bool IsEnabled { get { return isEnabled; } }

	void Start() {
		text = transform.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();

		if (!isEnabledAtStart) {
			Disable();
		}
	}

	public void Enable() {
		Color color = text.color;
		Color newColor = new Color(color.r, color.g, color.b, 1f);

		text.color = newColor;
		isEnabled = true;
	}

	public void Disable() {
		Color color = text.color;
		Color newColor = new Color(color.r, color.g, color.b, 0.5f);

		text.color = newColor;
		isEnabled = false;
	}

}
