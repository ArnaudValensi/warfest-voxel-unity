using UnityEngine;
using UnityEngine.UI;

public class ShiftShaderHue : MonoBehaviour {

	public int speed = 5;

	Material material;
	float hue = 0f;

	void Start() {
		material = GetComponent<Image>().material;
	}

	void Update() {
		hue = (hue + speed) % 360;
		material.SetFloat("_HueShift", hue);
	}

	void OnApplicationQuit() {
		material.SetFloat("_HueShift", 0f);
	}

}
