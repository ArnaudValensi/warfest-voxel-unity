using UnityEngine;
using System.Collections.Generic;

namespace Warfest {
	public class VoxelTerrain : MonoBehaviour {

		[SerializeField]
		int mapWidth = 128; // 8 chunks of 16 voxels
		[SerializeField]
		int mapHeight = 128;

		[SerializeField]
		Transform chunksHodler = null;

		[SerializeField]
		GameObject chunkPrefab;

		int chunkSize = 16;
		Dictionary<Pos, TerrainChunk> terrainChunks;

		void Start() {
			terrainChunks = new Dictionary<Pos, TerrainChunk>();
		}

		public void AddVoxel(Pos pos, Color32 color) {
			Pos chunkPos = pos.ContainingChunkCoordinates(chunkSize);
			Pos posInChunk = pos.ToLocalChunkCoordinates(chunkSize);
			TerrainChunk terrainChunk;

			Debug.LogFormat("[VoxelTerrain] chunkPos: {0}, posInChunk: {1}", chunkPos, posInChunk);

			if (!terrainChunks.TryGetValue(chunkPos, out terrainChunk)) {
				GameObject chunkObj = Instantiate(
					chunkPrefab,
					chunkPos,
					chunkPrefab.transform.rotation,
					chunksHodler
				);

				terrainChunk = chunkObj.GetComponent<TerrainChunk>();
				terrainChunk.Init(chunkSize);

				terrainChunks.Add(chunkPos, terrainChunk);
			}

			terrainChunk.SetVoxel(posInChunk, Voxel.Type.Solid, color);
		}

		// NOTE: could check if all voxels are air and remove it.
		public void RemoveVoxel(Pos pos) {
			Pos chunkPos = pos.ContainingChunkCoordinates(chunkSize);
			Pos posInChunk = pos.ToLocalChunkCoordinates(chunkSize);
			TerrainChunk terrainChunk;

			Debug.LogFormat("[VoxelTerrain] chunkPos: {0}, posInChunk: {1}", chunkPos, posInChunk);

			if (terrainChunks.TryGetValue(chunkPos, out terrainChunk)) {
				terrainChunk.SetVoxel(posInChunk, Voxel.Type.Air, Color.black);
			}

		}

	}
}
