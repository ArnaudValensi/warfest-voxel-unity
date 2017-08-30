using UnityEngine;
using System.Collections.Generic;

// TODO: Load existing texture at startup.
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
		}

		void OnEnable() {
			Debug.Log("[ColorTexture] OnEnable");
		}

		public void Init() {
			texture = new Texture2D(size, size);
			colorToUv = new Dictionary<Color32, Vector2>();
			colorIndex = 0;

			texture.filterMode = FilterMode.Point;
			texture.Apply();
		}

		public void LoadTexture(Texture2D textureToLoad) {
			colorIndex = 0;
			AddColors(textureToLoad.GetPixels32());
		}

		public void AddColor(Color32 color) {
			int x = colorIndex % size;
			int y = colorIndex / size;

			if (!colorToUv.ContainsKey(color)) {
				texture.SetPixel(x, y, color);
				colorToUv.Add(color, new Vector2((float)x / size, (float)y / size));

				colorIndex++;
			}
		}

		public void AddColors(Color32[] colors) {
			for (int i = 0; i < colors.Length; i++) {
				AddColor(colors[i]);
			}

			texture.Apply();
		}

		public Vector2 GetColorUV(Color32 color) {
			if (color == Color.black) {
				Debug.Log("[ColorTexture] black color was asked, should't happen");
			}

			return colorToUv[color];
		}

		public void SaveTextureToFile() {
			string filename = Application.dataPath + "/StreamingAssets/texture.png";

			System.IO.File.WriteAllBytes(filename, texture.EncodeToPNG());
			Debug.LogFormat("Texture save in {0}", filename);
		}

	}
}
