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

				RLEEncode(chunk.voxels, writer, sizeX, sizeY, sizeZ);
			}
		}

		// Run Length Encoding
		static void RLEEncode(Voxel[,,] voxels, BinaryWriter writer, int sizeX, int sizeY, int sizeZ) {
			int lastColor = 0;
			int currentColor;
			short consecutiveTypes = 0;
			byte lastType = byte.MaxValue;
			byte currentType;

			for (int z = 0; z < sizeZ; z++) {
				for (int y = 0; y < sizeY; y++) {
					for (int x = 0; x < sizeX; x++) {
						Voxel voxel = voxels[x, y, z];

						currentColor = Color32ToInt(voxel.color);
						currentType = (byte)voxel.type;
						if (currentColor == lastColor && currentType == lastType) {
							consecutiveTypes++;
						} else {
							if (consecutiveTypes > 0) {
								writer.Write(consecutiveTypes);
								writer.Write(lastType);
								writer.Write(lastColor);
							}
							lastColor = currentColor;
							lastType = currentType;
							consecutiveTypes = 1;
						}
					}
				}
			}

			writer.Write(consecutiveTypes);
			writer.Write(lastType);
			writer.Write(lastColor);
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

				RLEDecode(reader, sizeX, sizeY, sizeZ, posX, posY, posZ);
			}
		}

		// Run Length Encoding Decoding
		void RLEDecode(
			BinaryReader binaryReader, int sizeX, int sizeY, int sizeZ, int posX, int posY, int posZ
		) {
			int x = 0;
			int y = 0;
			int z = 0;
			byte type;
			int intColor;
			Color32 color;
			short count;
			int length = sizeX * sizeY * sizeZ;

			for (int i = 0; i < length;) {
				count = binaryReader.ReadInt16();
				type = binaryReader.ReadByte();
				intColor = binaryReader.ReadInt32();
				color = IntToColor32(intColor);

				for (int j = 0; j < count; j++) {
					// Add voxel if solid
					if ((Voxel.Type)type == Voxel.Type.Solid) {
						voxelTerrain.AddVoxel(
							new Pos(posX + x, posY + y, posZ + z),
							color
						);
					}

					x = (x + 1) % sizeX;

					if (x == 0) {
						y = (y + 1) % sizeY;

						if (y == 0) {
							z++;
						}
					}

					i++;
				}
			}
		}

		static int Color32ToInt(Color32 color) {
			int intColor;

			intColor = color.r << 24;
			intColor += color.g << 16;
			intColor += color.b << 8;
			intColor += color.a;

			return intColor;
		}

		static Color32 IntToColor32(int intColor) {
			return new Color32(
				(byte)((intColor >> 24) & 0xFF),
				(byte)((intColor >> 16) & 0xFF),
				(byte)((intColor >> 8) & 0xFF),
				(byte)(intColor & 0xFF)
			);
		}

	}
}