using UnityEngine;

namespace Warfest {
	public class Player : MonoBehaviour {
		public float movementSpeed = 10;
		public float turningSpeed = 60;

		Animator animator;
		int speedHash;

		void Start() {
			animator = GetComponent<Animator>();
			speedHash = Animator.StringToHash("Speed");
		}

		void Update() {
			Vector2 movement = new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical")
			);
			float horizontal = movement.x * turningSpeed * Time.deltaTime;
			transform.Rotate(0, horizontal, 0);

			float vertical = movement.y * movementSpeed * Time.deltaTime;
			transform.Translate(0, 0, vertical);

			animator.SetFloat(speedHash, movement.y);
		}
	}
}
