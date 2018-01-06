using UnityEngine;
using TMPro;

public class NewWorldCanvas : MonoBehaviour {

	[SerializeField] TMP_InputField inputField;
	[SerializeField] UIButton buttonCreate;

	public void OnOpen() {
		inputField.Select();
		inputField.text = "";
		buttonCreate.Disable();
	}

	public void OnInputValueChange() {
		if (inputField.text.Length == 0 && buttonCreate.IsEnabled) {
			buttonCreate.Disable();
		} else if (!buttonCreate.IsEnabled) {
			buttonCreate.Enable();
		}
	}

}
