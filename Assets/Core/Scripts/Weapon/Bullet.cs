using UnityEngine;

namespace Warfest {
	public class Bullet : MonoBehaviour {

		[SerializeField]
		float timeToDestroy = 10f;

		[SerializeField]
		float force = 1000f;

		[SerializeField]
		GameObject mesh;

		[SerializeField]
		GameObject particle;

		Rigidbody rbody;

		void Start() {
			rbody = GetComponent<Rigidbody>();

			rbody.AddRelativeForce(0, 0, force);
			Destroy(gameObject, timeToDestroy);
		}

		void OnCollisionEnter(Collision collision) {
			particle.SetActive(true);
			mesh.SetActive(false);

			Destroy(gameObject, 2f);
		}

	}
}
