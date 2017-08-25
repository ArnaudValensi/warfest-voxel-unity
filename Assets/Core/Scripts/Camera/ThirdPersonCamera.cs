using UnityEngine;

namespace Warfest {
	public class ThirdPersonCamera : MonoBehaviour {

		[SerializeField]
		float distanceAway = 3f;

		[SerializeField]
		Transform followTransform = null;

		[Range(0f, 10f)]
		[SerializeField]
		float turnSpeed = 1.5f;
		// How fast the rig will rotate from user input.

		[SerializeField]
		float tiltMax = 75f;
		// The maximum value of the x axis rotation of the pivot.

		[SerializeField]
		float tiltMin = 45f;
		// The minimum value of the x axis rotation of the pivot.

		// The rig's y axis rotation.
		float tiltAngle;

		// How far in front of the pivot the character's look target is.
		Vector3 pivotEulers;
		Quaternion pivotTargetRot;
		Quaternion transformTargetRot;
		Transform pivot;
		Transform mainCamera;
		float lookAngle;

		void Awake() {
			pivot = transform.Find("Pivot");
			mainCamera = pivot.Find("Main Camera");

			pivotEulers = pivot.rotation.eulerAngles;
			pivotTargetRot = pivot.transform.localRotation;
			transformTargetRot = transform.localRotation;

			mainCamera.localPosition = new Vector3(mainCamera.position.x, mainCamera.position.y, -distanceAway);
		}

		void Update() {
			HandleRotationMovement();
		}

		void LateUpdate() {
			// Follow target.
			transform.position = followTransform.position;
		}

		void HandleRotationMovement() {
			if (Time.timeScale < float.Epsilon) {
				return;
			}

			float x = Input.GetAxis("Mouse X");
			float y = Input.GetAxis("Mouse Y");

			// Adjust the look angle by an amount proportional to the turn speed and horizontal input.
			lookAngle += x * turnSpeed;

			// Rotate the rig (the root object) around Y axis only:
			transformTargetRot = Quaternion.Euler(0f, lookAngle, 0f);

			// on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
			tiltAngle -= y * turnSpeed;
			// and make sure the new value is within the tilt range
			tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);

			// Tilt input around X is applied to the pivot (the child of this object)
			pivotTargetRot = Quaternion.Euler(tiltAngle, pivotEulers.y, pivotEulers.z);

			pivot.localRotation = pivotTargetRot;
			transform.localRotation = transformTargetRot;
		}

	}
}