using UnityEngine;

namespace Warfest {
	public class AutoRotate : MonoBehaviour {

		[SerializeField]
		float speed = 1f;

		void Update() {
			transform.RotateAroundY(speed * Time.deltaTime);
		}

	}
}