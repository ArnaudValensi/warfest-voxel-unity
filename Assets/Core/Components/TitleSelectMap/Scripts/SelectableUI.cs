using UnityEngine;

public class SelectableUI : MonoBehaviour {

	[SerializeField] bool isSelectable = false;
	[SerializeField] bool isDisabled = false;
	public bool IsDisabled { get { return isDisabled; } }

	GameObject selection;
	GameObject selected;

	void Start() {
		selection = transform.Find("Selection").gameObject;

		if (isSelectable) {
			selected = transform.Find("Selected").gameObject;
		}
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

	public void Select() {
		if (isDisabled || !isSelectable) {
			return;
		}
		selected.SetActive(true);
	}

	public void Unselect() {
		selected.SetActive(false);
	}

}
