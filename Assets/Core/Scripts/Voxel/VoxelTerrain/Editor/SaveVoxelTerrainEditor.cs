using UnityEngine;
using UnityEditor;

namespace Warfest {
	[CustomEditor(typeof(SaveVoxelTerrain))]
	public class SaveVoxelTerrainEditor : Editor {

		public override void OnInspectorGUI() {
			// Grabbing the object this inspector is editing.
			var obj = (SaveVoxelTerrain)target;

			base.OnInspectorGUI();

			if (GUILayout.Button("Save")) {
				obj.Save();
			}

			if (GUILayout.Button("Load")) {
				obj.Load();
			}
		}

	}
}