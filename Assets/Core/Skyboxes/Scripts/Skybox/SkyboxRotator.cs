using UnityEngine;

public class SkyboxRotator : MonoBehaviour {
	public float RotationPerSecond = 1;
	public bool rotate = true;

	void Update() {
		if (rotate) {
			RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationPerSecond);
		}
	}

	void OnApplicationQuit() {
		RenderSettings.skybox.SetFloat("_Rotation", 0);
	}

}