using UnityEngine;

namespace Warfest {
	public class PlayerDetect : MonoBehaviour {

		[SerializeField]
		Material onPlayerMaterial = null;

		MeshRenderer meshRenderer;
		Material originalMaterial;

		void Start() {
			meshRenderer = transform.parent.GetComponent<MeshRenderer>();
		}

		void OnTriggerEnter(Collider other) {
			if (other.GetComponent<Player>() == null) {
				return;
			}

			originalMaterial = meshRenderer.sharedMaterial;
			meshRenderer.sharedMaterial = onPlayerMaterial;
		}

		void OnTriggerExit(Collider other) {
			if (other.GetComponent<Player>() == null) {
				return;
			}

			meshRenderer.sharedMaterial = originalMaterial;
		}

	}
}
