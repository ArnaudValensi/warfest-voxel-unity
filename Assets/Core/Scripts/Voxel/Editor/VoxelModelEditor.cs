using UnityEditor;
using UnityEngine;

namespace Warfest {
	[CustomEditor(typeof(VoxelModel))]
	public class VoxelModelEditor : Editor {
		int choiceIndex = -1;

		public override void OnInspectorGUI() {
			var voxelModel = target as VoxelModel;

			// Draw the default inspector
			DrawDefaultInspector();

			if (!voxelModel.IsStarted) {
				return;
			}

			string[] modelNames = voxelModel.GetAvailableModelNames();

			if (modelNames.Length > 0) {
				if (choiceIndex == -1) {
					choiceIndex = ArrayUtility.IndexOf(modelNames, voxelModel.modelName);
				}

				choiceIndex = EditorGUILayout.Popup(choiceIndex, modelNames);

				if (voxelModel.modelName != modelNames[choiceIndex]) {
					Debug.Log("[VoxelModelEditor] Update selection");

					// Update the selected choice in the underlying object
					voxelModel.SetModel(modelNames[choiceIndex]);

					// Save the changes back to the object
					EditorUtility.SetDirty(target);
				}
			}
		}
	}
}
