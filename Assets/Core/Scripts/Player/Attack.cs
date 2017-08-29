using UnityEngine;

namespace Warfest {
	public class Attack : MonoBehaviour {

		[SerializeField]
		Transform rightHand;

		Gun gun;

		void Start() {
			gun = rightHand.GetComponentInChildren<Gun>();
		}

		void Update () {
			if (Input.GetMouseButtonDown(0)) {
				DoAttack();
			}
		}

		void DoAttack() {
			gun.Shoot();
		}

	}
}
