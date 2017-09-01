using UnityEngine;

namespace Warfest {
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class TerrainChunk : MonoBehaviour {

		Chunk chunk;
		public Chunk Chunk { get { return chunk; } }
		MeshFilter meshFilter;
		MeshCollider meshCollider;
		MeshRenderer meshRenderer;
		ColorTexture colorTexture;
		VoxelMeshBuilder voxelMeshBuilder;
		ChunkSimplifier chunkSimplifier;

		public void Init(int chunkSize) {
			meshFilter = GetComponent<MeshFilter>();
			meshCollider = GetComponent<MeshCollider>();
			meshRenderer = GetComponent<MeshRenderer>();
			colorTexture = GameManager.Instance.GetColorTexture();
			voxelMeshBuilder = GameManager.Instance.GetVoxelMeshBuilder();
			chunkSimplifier = GameObject.Find("/Managers/ChunkSimplifier").GetComponent<ChunkSimplifier>();

			meshRenderer.sharedMaterial.mainTexture = colorTexture.Texture;

			chunk = new Chunk(chunkSize, chunkSize, chunkSize);
		}

		public void SetVoxel(Pos pos, Voxel.Type type, Color32 color) {
//			colorTexture.AddColor(color);
			chunk.SetVoxel(pos, type, color);
			Refresh();
		}

		void Refresh() {
//			MeshData meshData = voxelMeshBuilder.BuildMesh(chunk);
//			voxelMeshBuilder.RenderMesh(meshData, meshFilter, meshCollider);

			MeshData meshData = chunkSimplifier.BuildMesh(chunk);
			chunkSimplifier.RenderMesh(meshData, meshFilter, meshCollider);
		}

	}
}
