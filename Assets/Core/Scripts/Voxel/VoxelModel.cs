using UnityEngine;

namespace Warfest {
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class VoxelModel : MonoBehaviour {

		[SerializeField] string modelName = null;

		Chunk chunk;

		GameResources gameResources;
		ChunkSimplifier chunkSimplifier;
		ColorTexture colorTexture;

		MeshFilter meshFilter;
		MeshCollider meshCollider;
		MeshRenderer meshRenderer;

		void Start() {
			Debug.Log("[VoxelModel] Start");

			gameResources = GameObject.Find("/Managers/GameResources").GetComponent<GameResources>();
			chunkSimplifier = GameObject.Find("/Managers/ChunkSimplifier").GetComponent<ChunkSimplifier>();
			colorTexture = GameObject.Find("/Managers/ColorTexture").GetComponent<ColorTexture>();

			meshFilter = GetComponent<MeshFilter>();
			meshCollider = GetComponent<MeshCollider>();
			meshRenderer = GetComponent<MeshRenderer>();

			LoadModel();
		}

//		void OnValidate() {
//			Debug.Log("[VoxelModel] OnValidate");
//
//			LoadModel();
//		}

		void LoadModel() {
			Debug.Log("[VoxelModel] LoadModel");

			if (string.IsNullOrEmpty(modelName)) {
				return;
			}

			chunk = gameResources.GetChunk(modelName);
			meshRenderer.sharedMaterial.mainTexture = colorTexture.Texture;

			meshFilter.sharedMesh = new Mesh();

			MeshData meshData = chunkSimplifier.BuildMesh(chunk);
			chunkSimplifier.RenderMesh(meshData, meshFilter, meshCollider);
		}

	}
}