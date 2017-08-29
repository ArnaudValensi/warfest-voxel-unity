using UnityEngine;

namespace Warfest {
	[RequireComponent(typeof(Animator))]
	public class IKRightHand : MonoBehaviour {

		Animator animator;

		[SerializeField]
		bool ikActive = true;

		[SerializeField]
		Transform rightHandObj = null;

		[SerializeField]
		Transform lookObj = null;

		void Start() {
			animator = GetComponent<Animator>();
		}

		//a callback for calculating IK
		void OnAnimatorIK(int layerIndex) {
			Debug.Log("[IKRightHand] OnAnimatorIK");
			if (animator) {

				//if the IK is active, set the position and rotation directly to the goal.
				if (ikActive) {

					// Set the look target position, if one has been assigned
					if (lookObj != null) {
						Debug.Log("look at");

						animator.SetLookAtWeight(1);
						animator.SetLookAtPosition(lookObj.position);
					}

					// Set the right hand target position and rotation, if one has been assigned
					if (rightHandObj != null) {
						animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
						animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
						animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
						animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
					}

				}

				//if the IK is not active, set the position and rotation of the hand and head back to the original position
				else {
					animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
					animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
					animator.SetLookAtWeight(0);
				}
			}
		}

	}
}
