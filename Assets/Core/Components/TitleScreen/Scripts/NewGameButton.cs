using UnityEngine;
using UnityEngine.UI;

namespace Warfest {
	public class NewGameButton : MonoBehaviour {

		void Start() {
			GetComponent<Button>().onClick.AddListener(OnNewGame);
		}

		public void OnNewGame() {
			GameManager.Instance.NewGame();
		}

	}
}
