using UnityEngine;
using System.Collections.Generic;

public class ColorTexture : MonoBehaviour {

	Texture2D texture;

	public Texture2D Texture { get { return texture; } }

	Dictionary<Color32, Vector2> colorToUv;

	[SerializeField] int size = 0;

	public int Size { get { return size; } }

	public void AddColors(Color32[] colors) {
		texture = new Texture2D(size, size);
		colorToUv = new Dictionary<Color32, Vector2>();

		for (int i = 0; i < colors.Length; i++) {
			int x = i % size;
			int y = i / size;
			Color32 color = colors[i];

			texture.SetPixel(x, y, color);
			colorToUv.Add(color, new Vector2((float)x / size, (float)y / size));
		}

		texture.filterMode = FilterMode.Point;
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
