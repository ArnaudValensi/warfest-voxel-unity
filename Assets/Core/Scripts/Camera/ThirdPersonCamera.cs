using UnityEngine;

namespace Warfest {
	public class ThirdPersonCamera : MonoBehaviour {

		[SerializeField]
		Transform followTransform = null;

//		[SerializeField]
//		Vector3 offset = new Vector3(0f, 1.5f, 0f);

//		[SerializeField]
//		float rotateSpeed = 1f;

		[SerializeField]
		float distanceUp = 2f;

		[SerializeField]
		float distanceAway = 3f;

		Vector3 lookDir;
		Vector3 targetPosition;

		// Smoothing and damping
		Vector3 velocityCamSmooth = Vector3.zero;
		[SerializeField]
		float camSmoothDampTime = 0.1f;


		void LateUpdate() {
//			Vector2 mouseInput = new Vector2(
//				Input.GetAxis("Mouse X"),
//				Input.GetAxis("Mouse Y")
//			);
//
//			Debug.Log(mouseInput);

			// Allow to target the head of the character instead of the feet
//			Vector3 characterOffset = followTransform.position + offset;
			Vector3 followPos = followTransform.position;

			// Calculate direction from camera to player, kill Y, and normalize to give a valid direction
			// with unit magnitude.
			lookDir = followPos - transform.position;
			lookDir.y = 0;
			lookDir.Normalize();


			// Setting the target position to be the correct offset from the target.
			targetPosition = followPos + Vector3.up * distanceUp - lookDir * distanceAway;

			Debug.DrawRay(transform.position, lookDir, Color.green);
			Debug.DrawLine(followTransform.position, targetPosition, Color.magenta);

			CompensateForWalls(followPos, ref targetPosition);
			SmoothPostion(transform.position, targetPosition);

			// Make sure the camera is looking the right way.
			transform.LookAt(followTransform);
		}

		void SmoothPostion(Vector3 fromPos, Vector3 toPos) {
			// Making a smooth transition between the camera current position and the position
			// we want to be at.
			transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);

			// This line is an alternative way to smooth
			//transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
		}

		void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget) {
			Debug.DrawLine(fromObject, toTarget, Color.cyan);

			// Compensate for walls between camera
			RaycastHit wallHit;
			if (Physics.Linecast(fromObject, toTarget, out wallHit)) {
				Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
				toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
			}
		}

	}
}