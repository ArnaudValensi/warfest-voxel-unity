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

		// TODO: add pooling
		public void Shoot() {
			Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation, bulletsHolder);
		}

	}
}