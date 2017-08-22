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
//		MeshFilter meshFilter;
//		MeshCollider meshCollider;

		void Start() {
//			meshFilter = GetComponent<MeshFilter>();
//			meshCollider = GetComponent<MeshCollider>();
//
//			chunk = new Chunk(sizeX, sizeY, sizeZ);

			LoadQubicleFile();

//			MeshData meshData = VoxelMeshBuilder.BuildMesh(chunk);
//			VoxelMeshBuilder.RenderMesh(meshData, meshFilter, meshCollider);
		}

		void LoadQubicleFile() {
			string qbtPath = GameConfig.Instance.GetModelsPath() + "/Deer.qbt";

			Debug.Log("Path: " + qbtPath);

			new QBTFile(qbtPath);
		}

	}
}
