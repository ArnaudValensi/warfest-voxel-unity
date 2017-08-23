using UnityEngine;
using System.Collections.Generic;

namespace Warfest {
	[ExecuteInEditMode]
	public class ColorTexture : MonoBehaviour {

		Texture2D texture;
		public Texture2D Texture { get { return texture; } }

		Dictionary<Color32, Vector2> colorToUv;

		int colorIndex = 0;

		[SerializeField] int size = 0;

		public int Size { get { return size; } }

		void Awake() {
			Debug.Log("[ColorTexture] Awake");

			texture = new Texture2D(size, size);
			colorToUv = new Dictionary<Color32, Vector2>();

			texture.filterMode = FilterMode.Point;
			texture.Apply();
		}

		public void AddColors(Color32[] colors) {
			for (int i = 0; i < colors.Length; i++) {
				int x = (colorIndex + i) % size;
				int y = (colorIndex + i) / size;
				Color32 color = colors[i];

				Debug.Log("[ColorTexture] color: " + color);

				if (!colorToUv.ContainsKey(color)) {
					texture.SetPixel(x, y, color);
					colorToUv.Add(color, new Vector2((float)x / size, (float)y / size));

					colorIndex++;
				}
			}

			texture.Apply();
		}

		public Vector2 GetColorUV(Color32 color) {
			return colorToUv[color];
		}

		public void SaveTextureToFile() {
			string filename = Application.dataPath + "/StreamingAssets/texture.png";

			System.IO.File.WriteAllBytes(filename, texture.EncodeToPNG());
			Debug.LogFormat("Texture save in {0}", filename);
		}

	}
}
