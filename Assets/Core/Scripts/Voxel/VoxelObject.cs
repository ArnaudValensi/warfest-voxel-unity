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

		void Start() {
			meshFilter = GetComponent<MeshFilter>();
			meshCollider = GetComponent<MeshCollider>();

			chunk = new Chunk(sizeX, sizeY, sizeZ);
			chunk.SetVoxel(0, 0, 0, Voxel.Type.Solid);
			chunk.SetVoxel(1, 0, 0, Voxel.Type.Solid);

			MeshData meshData = VoxelMeshBuilder.BuildMesh(chunk);
			VoxelMeshBuilder.RenderMesh(meshData, meshFilter, meshCollider);
		}

	}
}
