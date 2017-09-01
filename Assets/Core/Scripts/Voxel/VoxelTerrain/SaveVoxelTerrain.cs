using UnityEngine;
using System.IO;

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
			saveFilename = string.Format("{0}/{1}.bin", savePath, filename);
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
				using (var writer = new BinaryWriter(writeStream)) {
					SaveData(writer, chunks);
				}

				writeStream.Close();
			}

			Debug.Log("[SaveVoxelTerrain] voxel terrain saved in: " + saveFilename);
		}

		void SaveData(BinaryWriter writer, Transform chunksTransform) {
			int nbChunks = chunksTransform.childCount;

			writer.Write(nbChunks);

			foreach (var chunkTransform in chunksTransform.GetChildren()) {
				Chunk chunk = chunkTransform.GetComponent<TerrainChunk>().Chunk;
				int sizeX = chunk.SizeX;
				int sizeY = chunk.SizeY;
				int sizeZ = chunk.SizeZ;
				Vector3 pos = chunkTransform.position;
				int posX = (int)pos.x;
				int posY = (int)pos.y;
				int posZ = (int)pos.z;

				writer.Write(sizeX);
				writer.Write(sizeY);
				writer.Write(sizeZ);
				writer.Write(posX);
				writer.Write(posY);
				writer.Write(posZ);

				for (int z = 0; z < sizeZ; z++) {
					for (int y = 0; y < sizeY; y++) {
						for (int x = 0; x < sizeX; x++) {
							Voxel voxel = chunk.voxels[x, y, z];
							byte type = (byte)voxel.type;
							byte[] colorArray = SerializeColor32(voxel.color);

							writer.Write(type);
							writer.Write(colorArray);
						}
					}
				}

			}
		}

		byte[] SerializeColor32(Color32 color) {
			byte[] colorArray = new byte[4];

			colorArray[0] = color.r;
			colorArray[1] = color.g;
			colorArray[2] = color.b;
			colorArray[3] = color.a;

			return colorArray;
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
				using (var reader = new BinaryReader(readStream)) {
					LoadData(reader);
				}
			}
		}

		void LoadData(BinaryReader reader) {
			int nbChunks = reader.ReadInt32();

			for (int i = 0; i < nbChunks; i++) {
				int sizeX = reader.ReadInt32();
				int sizeY = reader.ReadInt32();
				int sizeZ = reader.ReadInt32();
				int posX = reader.ReadInt32();
				int posY = reader.ReadInt32();
				int posZ = reader.ReadInt32();

				for (int z = 0; z < sizeZ; z++) {
					for (int y = 0; y < sizeY; y++) {
						for (int x = 0; x < sizeX; x++) {
							Voxel.Type type = (Voxel.Type)reader.ReadByte();
							byte[] colorArray = reader.ReadBytes(4);

							if (type == Voxel.Type.Solid) {
								Color32 color = DeserializeColor32(colorArray);

								Debug.Log("color: " + color);

								voxelTerrain.AddVoxel(
									new Pos(posX + x, posY + y, posZ + z),
									color
								);
							}
						}
					}
				}
			}
		}

		Color32 DeserializeColor32(byte[] colorArray) {
			return new Color32(
				colorArray[0],
				colorArray[1],
				colorArray[2],
				colorArray[3]
			);
		}

	}
}