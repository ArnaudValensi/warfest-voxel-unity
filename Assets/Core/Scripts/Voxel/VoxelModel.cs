using UnityEngine;

namespace Warfest {
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class VoxelModel : MonoBehaviour {

		public string modelName;

		bool isStarted = false;
		public bool IsStarted { get { return isStarted; } }

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

			isStarted = true;
		}

		void OnApplicationQuit() {
			Debug.Log("[VoxelModel] OnApplicationQuit");
		}

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

		public string[] GetAvailableModelNames() {
			return gameResources.GetAvailableModelNames();
		}

		public void SetModel(string modelName) {
			this.modelName = modelName;
			LoadModel();
		}

	}
}