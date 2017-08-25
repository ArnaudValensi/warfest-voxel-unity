using UnityEngine;

public static class MouseHelper {

	public static bool GetMousePositionInWorld(out Vector3 position, LayerMask layerMask = default(LayerMask)) {
		//create a ray cast and set it to the mouses cursor position in game
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 1000f, layerMask)) {
			position = hit.point;

			return true;
		} else {
			position = Vector2.zero;
		}

		return false;
	}

	public static float AngleBetweenPointAndMouse(Vector3 position) {
		//Get the Screen positions of the object
		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(position);

		//Get the Screen position of the mouse
		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

		//Get the angle between the points
		return AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
	}

	static float AngleBetweenTwoPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg + 180;
	}

}
