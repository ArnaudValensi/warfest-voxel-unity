using UnityEngine;
using TMPro;

public class MapSelectionButton : MonoBehaviour {

	TextMeshProUGUI nameText;
	TextMeshProUGUI dateText;

	public void Init(string name, string date) {
		nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		dateText = transform.Find("Date").GetComponent<TextMeshProUGUI>();

		nameText.SetText(name);
		dateText.SetText(date);
	}

}
