using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Warfest {
	public class GameManager : MonoBehaviourSingletonPersistent<GameManager> {

		public string titleScene;
		public string gameScene;

		bool gameLoading;

//		LuminaWorld world;

		//----------------------------------------------------------------------------
		// Lifecycle
		//----------------------------------------------------------------------------

		void Start() {
			if (SceneManager.GetActiveScene().name == "Game-01") {
				NewGame();
			}
		}

		//----------------------------------------------------------------------------
		// New Game
		//----------------------------------------------------------------------------

		// Method called when the "New Game" button is clicked on the title screen ("Start" scene).
		public void NewGame() {
			Debug.Log("[GameManager] NewGame");

			if (!gameLoading) {
				gameLoading = true;

				SceneManager.sceneLoaded -= OnGameSceneLoaded;
				SceneManager.sceneLoaded += OnGameSceneLoaded;
				SceneManager.LoadSceneAsync(gameScene);
			}
		}

		public void QuitGame() {
			// In most cases termination of application under iOS should be left at the user discretion.
			// https://developer.apple.com/library/content/qa/qa1561/_index.html
			#if !UNITY_IPHONE
				Application.Quit(); // Do not work in editor mode.
			#endif
		}

		//----------------------------------------------------------------------------
		// Game States
		//----------------------------------------------------------------------------

		// Called by SceneManager.LoadSceneAsync when the scene is loaded (but not active yet).
		void OnGameSceneLoaded(Scene scene, LoadSceneMode mode) {
			SceneManager.sceneLoaded -= OnGameSceneLoaded;
			StartCoroutine(OnGameSceneActive());
		}

		// Called when the scene is loaded and active.
		IEnumerator OnGameSceneActive() {
			yield return null; // Wait one frame

			gameLoading = false;

			Debug.Log("[GameManager] Scene Loaded");

			InitGame();
		}

		void InitGame() {
//			world = GameObject.Find("/World").GetComponent<LuminaWorld>();
//
//			world.Init();
		}

		//----------------------------------------------------------------------------
		// Methods
		//----------------------------------------------------------------------------

		public void Clean() {
		}

		public void SaveGame() {
//			saveManager.SaveGame();
//			Debug.Log("[GameManager] Game saved in " + saveManager.SavePath);
		}

		public void LoadGame() {
//			saveManager.LoadGame();
//			Debug.Log("[GameManager] Game loaded from " + saveManager.SavePath);

			InitGame();
		}

		//----------------------------------------------------------------------------
		// Accessors
		//----------------------------------------------------------------------------

//		public LuminaWorld GetLuminaWorld() {
//			return world;
//		}

	}
}
