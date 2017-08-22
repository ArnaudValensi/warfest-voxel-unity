using UnityEngine;

namespace Warfest {
	public class Chunk {

		public Voxel[,,] voxels;

		public int SizeX { get { return voxels.GetLength(0); } }
		public int SizeY { get { return voxels.GetLength(1); } }
		public int SizeZ { get { return voxels.GetLength(2); } }

		public Chunk(int sizeX, int sizeY, int sizeZ) {
			voxels = new Voxel[sizeX, sizeY, sizeZ];

			for (int x = 0; x < sizeX; x++) {
				for (int y = 0; y < sizeY; y++) {
					for (int z = 0; z < sizeZ; z++) {
						voxels[x, y, z] = new Voxel(Voxel.Type.Air, Vector2.zero);
					}
				}
			}
		}

		public void SetVoxel(int x, int y, int z, Voxel.Type type, Vector2 colorUv) {
			voxels[x, y, z].type = type;
			voxels[x, y, z].colorUv = colorUv;
		}

		public void SetVoxel(Pos pos, Voxel.Type type, Vector2 colorUv) {
			SetVoxel(pos.x, pos.y, pos.z, type, colorUv);
		}

		public Voxel GetVoxel(int x, int y, int z) {
			return voxels[x, y, z];
		}

		public Voxel GetVoxel(Pos pos) {
			return GetVoxel(pos.x, pos.y, pos.z);
		}

		public bool IsInRange(int x, int y, int z) {
			return x >= 0 && x < SizeX && y >= 0 && y < SizeY && z >= 0 && z < SizeZ;
		}

		public bool IsInRange(Pos pos) {
			return IsInRange(pos.x, pos.y, pos.z);
		}

		public bool IsOutOfRange(int x, int y, int z) {
			return !IsInRange(x, y, z);
		}

		public bool IsOutOfRange(Pos pos) {
			return IsOutOfRange(pos.x, pos.y, pos.z);
		}

	}
}
