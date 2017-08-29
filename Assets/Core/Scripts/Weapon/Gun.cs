using UnityEngine;

namespace Warfest {
	public class Gun : MonoBehaviour {

		[SerializeField]
		GameObject bulletPrefab = null;

		Transform spawnPosition;
		Transform bulletsHolder;

		void Start() {
			spawnPosition = transform.Find("BulletSpawn");
			bulletsHolder = GameObject.Find("/Managers/BulletPool").transform;
		}

		public void Shoot() {
			Debug.Log("[Gun] Shoot");

			Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation, bulletsHolder);
		}

	}
}