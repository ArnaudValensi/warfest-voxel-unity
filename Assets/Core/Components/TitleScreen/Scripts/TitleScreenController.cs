using UnityEngine;

public class TitleScreenController : MonoBehaviour {

	[SerializeField] GameObject titleCanvas;
	[SerializeField] GameObject mapSelectionCanvas;

	public void GoToMapSelection() {
		titleCanvas.SetActive(false);
		mapSelectionCanvas.SetActive(true);
	}

	public void GoToTitle() {
		mapSelectionCanvas.SetActive(false);
		titleCanvas.SetActive(true);
	}

}
