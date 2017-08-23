using UnityEngine;
using UnityEditor;

namespace Warfest {
	[CustomEditor(typeof(ColorTexture))]
	public class ColorTextureEditor : Editor {

		bool hasPreview;
		Texture2D previewTexture;

		public override void OnInspectorGUI() {
			// Grabbing the object this inspector is editing.
			ColorTexture colorTexture = (ColorTexture)target;

			base.OnInspectorGUI();

			if (GUILayout.Button("Save Texture")) {
				colorTexture.SaveTextureToFile();
			}

			if (GUILayout.Button("Preview")) {
				var texture = colorTexture.Texture;

				if (texture != null) {
					previewTexture = ScaleTexture(texture, 16);
					hasPreview = true;

				}
			}

			if (hasPreview) {
				GUILayout.Label(previewTexture);
			}
		}

		Texture2D ScaleTexture(Texture2D texture, int scaleFactor) {
			int baseWidth = texture.width;
			int baseHeight = texture.height;
			int newWidth = baseWidth * scaleFactor;
			int newHeight = baseHeight * scaleFactor;
			var scaledTexture = new Texture2D(newWidth, newHeight);

			for (int y = 0; y < baseHeight; y++) {
				for (int x = 0; x < baseWidth; x++) {
					Color32 color = texture.GetPixel(x, y);

					Debug.Log("[ColorTextureEditor] Color: " + color);

					for (int j = 0; j < scaleFactor; j++) {
						for (int i = 0; i < scaleFactor; i++) {
							scaledTexture.SetPixel(x * scaleFactor + i, y * scaleFactor + j, color);
						}
					}
				}
			}

			scaledTexture.Apply();

			return scaledTexture;
		}

	}
}