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
		Vector3 jumpVelocity = Vector3.zero;

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

			transform.rotation = cameraTransform.rotation;

			// Jump
			if (controller.isGrounded) {
				grounded = true;
				jumpVelocity.y = -gravity * Time.deltaTime;

				if (Input.GetButtonDown("Jump")) {
					jumpVelocity.y = jumpForce;
				}
			} else {
				grounded = false;

				// If it is falling
				if (jumpVelocity.y < 0) {
					jumpVelocity.y -= gravity * Time.deltaTime * fallCoef;
				} else {
					jumpVelocity.y -= gravity * Time.deltaTime;
				}
			}

			controller.Move(jumpVelocity + moveInCameraSpace);

			// Anims
			animator.SetFloat(animSpeedX, x);
			animator.SetFloat(animSpeedY, y);
		}
	}
}
