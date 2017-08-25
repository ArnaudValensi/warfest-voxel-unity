using UnityEngine;

namespace Warfest {
	public class Player : MonoBehaviour {
		public float movementSpeed = 0.2f;
		public float rotationSpeed = 1f;
		public Transform cameraTransform;

		Animator animator;
		int animSpeedX;
		int animSpeedY;

		void Start() {
			animator = GetComponent<Animator>();
			animSpeedX = Animator.StringToHash("SpeedX");
			animSpeedY = Animator.StringToHash("SpeedY");
		}

		void Update() {
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			Vector3 movement = new Vector3(x * movementSpeed, 0f, y * movementSpeed);

			// Convert input movement to camera space
			var moveInCameraSpace = cameraTransform.TransformDirection(movement);

			transform.position = transform.position + moveInCameraSpace;
			transform.rotation = cameraTransform.rotation;

//			transform.rotation = Quaternion.Slerp(transform.rotation, cameraTransform.rotation, Time.deltaTime * rotationSpeed);
//			transform.rotation = Quaternion.RotateTowards(
//				transform.rotation,
//				cameraTransform.rotation,
//				rotationSpeed * Time.deltaTime
//			);

			animator.SetFloat(animSpeedX, x);
			animator.SetFloat(animSpeedY, y);
		}
	}
}
