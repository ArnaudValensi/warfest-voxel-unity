using UnityEngine;
using TMPro;

public class NewWorldCanvas : MonoBehaviour {

	[SerializeField] TMP_InputField inputField;

	public void OnOpen() {
		inputField.Select();
		inputField.text = "";
	}

}
