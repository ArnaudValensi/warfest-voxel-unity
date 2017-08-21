using UnityEngine;
using Object = UnityEngine.Object;

public static class VectorExtensions {

	public static Vector2 Copy(this Vector2 vector) {
		return new Vector2(vector.x, vector.y);
	}

	public static Vector3 Copy(this Vector3 vector) {
		return new Vector3(vector.x, vector.y, vector.z);
	}

}