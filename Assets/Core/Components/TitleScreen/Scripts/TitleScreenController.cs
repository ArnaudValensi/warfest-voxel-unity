using UnityEngine;

public class TitleScreenController : MonoBehaviour {

	[SerializeField] GameObject titleCanvas;
	[SerializeField] GameObject mapSelectionCanvas;
	[SerializeField] GameObject newWorldCanvas;

	public void GoToMapSelection() {
		titleCanvas.SetActive(false);
		newWorldCanvas.SetActive(false);
		mapSelectionCanvas.SetActive(true);
	}

	public void GoToTitle() {
		mapSelectionCanvas.SetActive(false);
		titleCanvas.SetActive(true);
	}

	public void GoToNewWorldCreation() {
		mapSelectionCanvas.SetActive(false);
		newWorldCanvas.SetActive(true);
	}

}
