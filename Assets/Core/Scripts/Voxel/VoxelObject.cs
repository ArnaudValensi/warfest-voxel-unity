using UnityEngine;

namespace Warfest {
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class VoxelObject : MonoBehaviour {

		public int sizeX;
		public int sizeY;
		public int sizeZ;

		Chunk chunk;
		MeshFilter meshFilter;
		MeshCollider meshCollider;
		VoxelMeshBuilder voxelMeshBuilder;

		void Start() {
			meshFilter = GetComponent<MeshFilter>();
			meshCollider = GetComponent<MeshCollider>();
			voxelMeshBuilder = GameManager.Instance.GetVoxelMeshBuilder();

			chunk = new Chunk(sizeX, sizeY, sizeZ);
			chunk.SetVoxel(0, 0, 0, Voxel.Type.Solid, Color.black);
			chunk.SetVoxel(1, 0, 0, Voxel.Type.Solid, Color.black);

			MeshData meshData = voxelMeshBuilder.BuildMesh(chunk);
			voxelMeshBuilder.RenderMesh(meshData, meshFilter, meshCollider);
		}

	}
}
