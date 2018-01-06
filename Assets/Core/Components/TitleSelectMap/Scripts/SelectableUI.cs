using UnityEngine;

public class SelectableUI : MonoBehaviour {

	[SerializeField] bool isDisabled = false;
	public bool IsDisabled { get { return isDisabled; } set { isDisabled = value; } }

	GameObject selection;

	void Start() {
		selection = transform.Find("Selection").gameObject;
	}

	public void OnPointerEnter() {
		if (isDisabled) {
			return;
		}
		selection.SetActive(true);
	}

	public void OnPointerLeave() {
		if (isDisabled) {
			return;
		}
		selection.SetActive(false);
	}

}
