using UnityEngine;

namespace Warfest {
	public class Bullet : MonoBehaviour {

		[SerializeField]
		float timeToDestroy = 10f;

		[SerializeField]
		float force = 1000f;

		Rigidbody rbody;

		void Start() {
			rbody = GetComponent<Rigidbody>();

			rbody.AddRelativeForce(0, 0, force);
			Destroy(gameObject, timeToDestroy);
		}

	}
}
