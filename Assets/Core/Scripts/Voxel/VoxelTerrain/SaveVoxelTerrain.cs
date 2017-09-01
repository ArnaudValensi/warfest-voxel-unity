using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace Warfest {
	public class SaveVoxelTerrain : MonoBehaviour {

		[SerializeField]
		string filename = null;

		Transform chunks;
		string savePath;
		string saveFilename;
		VoxelTerrain voxelTerrain;

		void Start() {
			voxelTerrain = GameObject.Find("VoxelTerrain").GetComponent<VoxelTerrain>();
			chunks = transform.Find("Chunks");

			savePath = GameConfig.Instance.GetSavePath();
			saveFilename = string.Format("{0}/{1}.json", savePath, filename);
		}

		//----------------------------------------------------------------------------
		// Save
		//----------------------------------------------------------------------------

		public void Save() {
			Debug.Log("[SaveVoxelTerrain] Save");

			// Create directory if needed
			if (!Directory.Exists(savePath)) {
				Directory.CreateDirectory(savePath);
			}

			using (var writeStream = File.OpenWrite(saveFilename)) {
				using (var writer = new StreamWriter(writeStream)) {
					ChunksData chunksData = GetChunksData(chunks);
					string json = JsonUtility.ToJson(chunksData, true);

					writer.Write(json);
				}
				writeStream.Close();
			}

			Debug.Log("[SaveVoxelTerrain] voxel terrain saved in: " + saveFilename);
		}

		ChunksData GetChunksData(Transform chunksTransform) {
			ChunkData[] chunksData = new ChunkData[chunksTransform.childCount];

			int i = 0;
			foreach (var chunk in chunks.GetChildren()) {
				ChunkData chunkData = GetChunkData(chunk);

				chunksData[i] = chunkData;
				i++;
			}

			return new ChunksData {
				chunks = chunksData
			};
		}

		ChunkData GetChunkData(Transform chunkTransform) {
			Chunk chunk = chunkTransform.GetComponent<TerrainChunk>().Chunk;
			int sizeX = chunk.SizeX;
			int sizeY = chunk.SizeY;
			int sizeZ = chunk.SizeZ;
			Vector3 pos = chunkTransform.position;
			int x = (int)pos.x;
			int y = (int)pos.y;
			int z = (int)pos.z;

			return new ChunkData {
				sizeX = sizeX,
				sizeY = sizeY,
				sizeZ = sizeZ,
				x = x,
				y = y,
				z = z,
				voxels = GetVoxelsData(chunk.voxels, sizeX, sizeY, sizeZ)
			};
		}

		VoxelData[] GetVoxelsData(Voxel[,,] voxels, int sizeX, int sizeY, int sizeZ) {
			var voxelData = new List<VoxelData>();
			Voxel voxel;
			Color32 color;
			int intColor;

			for (int x = 0; x < sizeX; x++) {
				for (int y = 0; y < sizeY; y++) {
					for (int z = 0; z < sizeX; z++) {
						voxel = voxels[x, y, z];
						color = voxel.color;

						intColor = color.r << 24;
						intColor += color.g << 16;
						intColor += color.b << 8;
						intColor += color.a;

						if (voxel.type == Voxel.Type.Solid) {
							Debug.LogFormat("color: " + color);
						}

						voxelData.Add(new VoxelData {
							type = (int)voxel.type,
							color = intColor
						});
					}
				}
			}

			return voxelData.ToArray();
		}

		//----------------------------------------------------------------------------
		// Load
		//----------------------------------------------------------------------------

		public void Load() {
			Debug.Log("[SaveVoxelTerrain] Load");

			if (!Directory.Exists(savePath)) {
				Debug.LogWarning("No save at this path: " + savePath);
				return;
			}

			using (var readStream = File.OpenRead(saveFilename)) {
				string json;

				using(var reader = new StreamReader(readStream)) {
					json = reader.ReadToEnd();
				}

				ChunksData chunksData = JsonUtility.FromJson<ChunksData>(json);

				LoadChunksData(chunksData);
			}
		}

		void LoadChunksData(ChunksData chunksData) {
			ChunkData[] chunks = chunksData.chunks;

			for (int i = 0; i < chunks.Length; i++) {
				LoadChunkData(chunks[i]);
			}
		}

		void LoadChunkData(ChunkData chunkData) {
			VoxelData[] voxelsData = chunkData.voxels;
			int sizeX = chunkData.sizeX;
			int sizeY = chunkData.sizeY;
			int posX = (int)chunkData.x;
			int posY = (int)chunkData.y;
			int posZ = (int)chunkData.z;

			for (int i = 0; i < voxelsData.Length; i++) {
				VoxelData voxelData = voxelsData[i];
				Voxel.Type type = (Voxel.Type)voxelData.type;

				if (type == Voxel.Type.Solid) {
					int intColor = voxelData.color;
					int z = i % sizeX;
					int y = (i % (sizeX * sizeY)) / sizeX;
					int x = i / (sizeX * sizeY);
					Color32 color = new Color32(
						(byte)((intColor >> 24) & 0xFF),
						(byte)((intColor >> 16) & 0xFF),
						(byte)((intColor >> 8) & 0xFF),
						(byte)(intColor & 0xFF)
					);

					Debug.Log(color);

					voxelTerrain.AddVoxel(
						new Pos(posX + x, posY + y, posZ + z),
						color
					);
				}
			}
		}

		//----------------------------------------------------------------------------
		// Data
		//----------------------------------------------------------------------------

		[Serializable]
		public class ChunksData {
			public ChunkData[] chunks;
		}

		[Serializable]
		public class ChunkData {
			public int sizeX;
			public int sizeY;
			public int sizeZ;
			public int x;
			public int y;
			public int z;
			public VoxelData[] voxels;
		}

		[Serializable]
		public class VoxelData {
			public int type;
			public int color;
		}

	}
}