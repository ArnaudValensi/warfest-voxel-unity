using UnityEngine;

public class CursorLock : MonoBehaviour {

	void Start () {
		SetCursorLock(true);
	}
	
	void Update () {
		if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0)) {
			SetCursorLock(true);
		}
	}

	void SetCursorLock(bool locked) {
		Cursor.lockState = (locked) ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = CursorLockMode.Locked != Cursor.lockState;
	}

}
