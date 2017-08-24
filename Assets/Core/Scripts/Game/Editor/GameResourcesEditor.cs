using UnityEditor;
using UnityEngine;

namespace Warfest {
	[CustomEditor(typeof(GameResources))]
	public class GameResourcesEditor : Editor {
		int choiceIndex = -1;

		public override void OnInspectorGUI() {
			var obj = target as GameResources;

			// Draw the default inspector
			DrawDefaultInspector();

			if (GUILayout.Button("Generate All Models")) {
				obj.GenerateModels();
			}

			string[] modelNames = obj.GetAvailableModelNames();

			if (modelNames.Length > 0) {
				choiceIndex = EditorGUILayout.Popup(choiceIndex, modelNames);
			}

			if (modelNames.Length > 0 && choiceIndex != -1 && GUILayout.Button("Generate Prefab")) {
				obj.GeneratePrefab(modelNames[choiceIndex]);
			}

			Debug.Log("[GameResourcesEditor]");

		}
	}
}
