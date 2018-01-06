using UnityEngine;

public class InputField : MonoBehaviour {

	[SerializeField] GameObject unselectedLine;
	[SerializeField] GameObject selectedLine;

	public void OnSelect() {
		unselectedLine.SetActive(false);
		selectedLine.SetActive(true);
	}

	public void OnDeselect() {
		selectedLine.SetActive(false);
		unselectedLine.SetActive(true);
	}

}
