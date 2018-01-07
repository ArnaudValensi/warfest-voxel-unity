using UnityEngine;
using TMPro;
using System;

public class MapSelectionButton : MonoBehaviour {

	TextMeshProUGUI nameText;
	TextMeshProUGUI folderNameText;
	TextMeshProUGUI dateText;
	GameObject selected;

	public event Action OnClicked;

	public void Init(string name, string folderName, string date) {
		selected = transform.Find("Selected").gameObject;
		nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		folderNameText = transform.Find("FolderName").GetComponent<TextMeshProUGUI>();
		dateText = transform.Find("Date").GetComponent<TextMeshProUGUI>();

		nameText.SetText(name);
		folderNameText.SetText(folderName);
		dateText.SetText(date);
	}

	public void Clicked() {
		if (OnClicked != null) {
			OnClicked.Invoke();
		}
	}

	public void Select() {
		selected.SetActive(true);
	}

	public void Unselect() {
		selected.SetActive(false);
	}

}
