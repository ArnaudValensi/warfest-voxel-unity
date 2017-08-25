using UnityEngine;

public class FlyCam : MonoBehaviour {
	Vector3 movementVector;

	public enum RotationAxes {
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2

	}

	public int speed = 7;
	public int verticalSpeed = 7;

	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;
	public float minimumY = -60F;
	public float maximumY = 60F;

	static float rotationY = 0F;

	void Update() {
		KeyInput();

		if (Cursor.lockState == CursorLockMode.Locked) {
			MouseAxesInput();
		}
	}

	void KeyInput() {
		movementVector = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
			movementVector += Camera.main.transform.forward * speed;

		if (Input.GetKey(KeyCode.S))
			movementVector -= Camera.main.transform.forward * speed;

		if (Input.GetKey(KeyCode.A))
			movementVector -= Camera.main.transform.right * speed;

		if (Input.GetKey(KeyCode.D))
			movementVector += Camera.main.transform.right * speed;

		if (Input.GetKey(KeyCode.Space))
			movementVector += Camera.main.transform.up * verticalSpeed;

		if (Input.GetKey(KeyCode.LeftShift))
			movementVector -= Camera.main.transform.up * verticalSpeed;

		Camera.main.transform.position = Camera.main.transform.position + movementVector * Time.deltaTime;
	}

	void MouseAxesInput() {
		if (axes == RotationAxes.MouseXAndY) {
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		} else if (axes == RotationAxes.MouseX) {
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		} else {
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}

}
