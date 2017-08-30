using UnityEditor;
using UnityEngine;

namespace Warfest {
	[CustomEditor(typeof(GameResources))]
	public class GameResourcesEditor : Editor {
		public override void OnInspectorGUI() {
			var obj = target as GameResources;

			// Draw the default inspector
			DrawDefaultInspector();

			if (GUILayout.Button("Bake Texture From Model")) {
				obj.BakeTextureFromModel();
			}

			if (GUILayout.Button("Generate All Models")) {
				obj.GenerateModels();
			}

		}
	}
}
