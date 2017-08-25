using UnityEngine;

namespace Warfest {
	public class Player : MonoBehaviour {
		public float movementSpeed = 0.2f;
		public float rotationSpeed = 1f;
		public Transform cameraTransform;

		Animator animator;
		int animSpeedX;
		int animSpeedY;

		CharacterController controller;

		// Jump
		public float gravity = 30f;
		public float fallCoef = 2f;
		public float jumpForce = 14f;

		bool grounded = false;
		Vector3 velocity = Vector3.zero;

		void Start() {
			animator = GetComponent<Animator>();
			animSpeedX = Animator.StringToHash("SpeedX");
			animSpeedY = Animator.StringToHash("SpeedY");

			controller = GetComponent<CharacterController>();
		}

		void Update() {
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			bool jump = Input.GetButtonDown("Jump");

			// Movement
			Vector3 movement = new Vector3(
				x * movementSpeed * Time.deltaTime,
				0f,
				y * movementSpeed * Time.deltaTime
			);

			// Convert input movement to camera space
			var moveInCameraSpace = cameraTransform.TransformDirection(movement);

//			controller.Move(moveInCameraSpace);
//			transform.position = transform.position + moveInCameraSpace;
			transform.rotation = cameraTransform.rotation;

			animator.SetFloat(animSpeedX, x);
			animator.SetFloat(animSpeedY, y);

			// Jump
			if (controller.isGrounded) {
				grounded = true;
				velocity.y = -gravity * Time.deltaTime;

				if (Input.GetButtonDown("Jump")) {
					velocity.y = jumpForce;
				}
			} else {
				grounded = false;

				// If it is falling
				if (velocity.y < 0) {
					velocity.y -= gravity * Time.deltaTime * fallCoef;
				} else {
					velocity.y -= gravity * Time.deltaTime;
				}
			}

			controller.Move(velocity + moveInCameraSpace);
		}
	}
}
