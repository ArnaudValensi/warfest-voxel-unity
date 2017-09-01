using System.IO;
using UnityEngine;

namespace Warfest {
	public static class RunLengthEncoding {

		public static void RLEEncode(Voxel[,,] voxels, BinaryWriter writer, int sizeX, int sizeY, int sizeZ) {
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

		public static SaveVoxelTerrain.VoxelData[,,] RLEDecode(BinaryReader binaryReader, int sizeX, int sizeY, int sizeZ) {
			var voxelsData = new SaveVoxelTerrain.VoxelData[sizeX, sizeY, sizeZ];
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
					voxelsData[x, y, z] = new SaveVoxelTerrain.VoxelData {
						type = (Voxel.Type)type,
						color = color
					};

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

			return voxelsData;
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