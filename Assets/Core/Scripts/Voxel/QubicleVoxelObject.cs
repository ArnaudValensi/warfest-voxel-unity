using UnityEngine;

namespace Warfest {
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class QubicleVoxelObject : MonoBehaviour {

		public int sizeX;
		public int sizeY;
		public int sizeZ;

		Chunk chunk;
		MeshFilter meshFilter;
		MeshCollider meshCollider;

		void Start() {
			meshFilter = GetComponent<MeshFilter>();
			meshCollider = GetComponent<MeshCollider>();

			QBTFile.VoxelData[,,] qbtData = LoadQubicleFile().VoxelsData;

			chunk = new Chunk(qbtData.GetLength(0), qbtData.GetLength(1), qbtData.GetLength(2));
			for (int x = 0; x < qbtData.GetLength(0); x++) {
				for (int y = 0; y < qbtData.GetLength(1); y++) {
					for (int z = 0; z < qbtData.GetLength(2); z++) {
						if (qbtData[x, y, z].m != 0) {
							chunk.SetVoxel(x, y, z, Voxel.Type.Solid);
						}
					}
				}
			}

			MeshData meshData = VoxelMeshBuilder.BuildMesh(chunk);
			VoxelMeshBuilder.RenderMesh(meshData, meshFilter, meshCollider);
		}

		QBTFile LoadQubicleFile() {
			string qbtPath = GameConfig.Instance.GetModelsPath() + "/Deer.qbt";

			Debug.Log("Path: " + qbtPath);

			return new QBTFile(qbtPath);
		}

	}
}
