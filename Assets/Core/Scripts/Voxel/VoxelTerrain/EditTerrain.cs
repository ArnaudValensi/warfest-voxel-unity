using UnityEngine;

namespace Warfest {
	public class EditTerrain : MonoBehaviour {

		[SerializeField]
		LayerMask hitLayerMask;

		VoxelTerrain voxelTerrain;

		void Start() {
			voxelTerrain = GameObject.Find("VoxelTerrain").GetComponent<VoxelTerrain>();
		}

		void Update() {
			if (Input.GetMouseButtonDown(0)) {
				SetVoxel();
			}
		}

		void SetVoxel() {
			Transform cameraTransform = Camera.main.transform;
			RaycastHit hit;

			Debug.Log("[EditTerrain] SetVoxel");

			if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 100, hitLayerMask)) {
				Pos pos = GetVoxelPos(hit, true);

				Debug.Log("[EditTerrain] SetVoxel, hit, pos: " + pos);

				voxelTerrain.SetVoxel(pos);
			}
		}

		static Pos GetVoxelPos(Vector3 pos)	{
			Pos voxelPos = new Pos(
				Mathf.RoundToInt(pos.x),
				Mathf.RoundToInt(pos.y),
				Mathf.RoundToInt(pos.z)
			);

			return voxelPos;
		}


		static Pos GetVoxelPos(RaycastHit hit, bool adjacent = false) {
			Vector3 pos = new Vector3(
				MoveWithinVoxel(hit.point.x, hit.normal.x, adjacent),
				MoveWithinVoxel(hit.point.y, hit.normal.y, adjacent),
				MoveWithinVoxel(hit.point.z, hit.normal.z, adjacent)
			);

			return GetVoxelPos(pos);
		}

		static float MoveWithinVoxel(float pos, float norm, bool adjacent = false) {
			if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f) {
				if (adjacent) {
					pos += (norm / 2);
				} else {
					pos -= (norm / 2);
				}
			}

			return pos;
		}

	}
}