using UnityEngine;
using UnityEditor;

namespace Warfest {
	[CustomEditor(typeof(ColorTexture))]
	public class ColorTextureEditor : Editor {

		public override void OnInspectorGUI() {
			// Grabbing the object this inspector is editing.
			ColorTexture colorTexture = (ColorTexture)target;

			base.OnInspectorGUI();

			if (GUILayout.Button("Save Texture")) {
				colorTexture.SaveTextureToFile();
			}
		}

	}
}