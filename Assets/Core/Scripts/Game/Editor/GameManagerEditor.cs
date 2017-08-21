using UnityEngine;
using UnityEditor;

namespace Warfest {
	[CustomEditor(typeof(GameManager))]
	public class GameManagerEditor : Editor {
		
		public override void OnInspectorGUI() {
			// Grabbing the object this inspector is editing.
			GameManager gameManager = (GameManager)target;

			base.OnInspectorGUI();

			if (GUILayout.Button("New Game")) {
				gameManager.NewGame();
			}

			if (GUILayout.Button("Save Game")) {
				gameManager.SaveGame();
			}

			if (GUILayout.Button("Load Game")) {
				gameManager.LoadGame();
			}

			if (GUILayout.Button("Clean")) {
				gameManager.Clean();
			}
		}

	}
}