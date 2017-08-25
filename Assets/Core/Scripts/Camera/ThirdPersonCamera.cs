using UnityEngine;

namespace Warfest {
	public class ThirdPersonCamera : MonoBehaviour {

		//		[SerializeField]
		//		float distanceUp = 2f;
		
		[SerializeField]
		float distanceAway = 3f;

		[SerializeField]
		Transform followTransform = null;

		[Range(0f, 10f)]
		[SerializeField]
		float m_TurnSpeed = 1.5f;
		// How fast the rig will rotate from user input.

		[SerializeField]
		float followSpeed = 1f;

		[SerializeField]
		float m_TiltMax = 75f;
		// The maximum value of the x axis rotation of the pivot.

		[SerializeField]
		float m_TiltMin = 45f;
		// The minimum value of the x axis rotation of the pivot.

		float m_LookAngle;
		// The rig's y axis rotation.
		float m_TiltAngle;
		// The pivot's x axis rotation.
		const float k_LookDistance = 100f;
		// How far in front of the pivot the character's look target is.
		Vector3 m_PivotEulers;
		Quaternion m_PivotTargetRot;
		Quaternion m_TransformTargetRot;
		Transform m_Pivot;
		Transform mainCamera;

		void Awake() {
			m_Pivot = transform.Find("Pivot");
			mainCamera = m_Pivot.Find("Main Camera");

			m_PivotEulers = m_Pivot.rotation.eulerAngles;
			m_PivotTargetRot = m_Pivot.transform.localRotation;
			m_TransformTargetRot = transform.localRotation;

			Debug.Log(distanceAway);

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
			m_LookAngle += x * m_TurnSpeed;

			// Rotate the rig (the root object) around Y axis only:
			m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

			// on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
			m_TiltAngle -= y * m_TurnSpeed;
			// and make sure the new value is within the tilt range
			m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);

			// Tilt input around X is applied to the pivot (the child of this object)
			m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y, m_PivotEulers.z);

			m_Pivot.localRotation = m_PivotTargetRot;
			transform.localRotation = m_TransformTargetRot;
		}

	}
}