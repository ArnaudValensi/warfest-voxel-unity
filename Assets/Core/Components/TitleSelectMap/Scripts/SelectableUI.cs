using UnityEngine;

public class SelectableUI : MonoBehaviour {

	GameObject selection;

	void Start() {
		selection = transform.Find("Selection").gameObject;
	}

	public void OnPointerEnter() {
		selection.SetActive(true);
	}

	public void OnPointerLeave() {
		selection.SetActive(false);
	}

}
