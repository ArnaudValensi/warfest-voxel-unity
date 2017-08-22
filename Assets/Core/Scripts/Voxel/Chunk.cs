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
						voxels[x, y, z] = new Voxel();
					}
				}
			}
		}

		public void SetVoxel(int x, int y, int z, Voxel.Type type, Color32 color) {
			voxels[x, y, z].type = type;
			voxels[x, y, z].color = color;
		}

		public void SetVoxel(Pos pos, Voxel.Type type, Color32 color) {
			SetVoxel(pos.x, pos.y, pos.z, type, color);
		}

		public Voxel GetVoxel(int x, int y, int z) {
			return voxels[x, y, z];
		}

		public Voxel GetVoxel(Pos pos) {
			return GetVoxel(pos.x, pos.y, pos.z);
		}

		public Voxel GetVoxelBasedOnPlan(int x, int y, int z, Direction direction) {
			switch (direction) {
			case Direction.south:
				return voxels[x, y, z];
			case Direction.north:
				return voxels[SizeX - x - 1, y, SizeZ - z - 1];
			case Direction.west:
				return voxels[SizeZ - z - 1, y, x];
			case Direction.east:
				return voxels[z, y, SizeX - x - 1];
			case Direction.up:
				return voxels[x, z, SizeY - y - 1];
			case Direction.down:
				return voxels[x, SizeZ - z - 1, y];
			default:
				throw new System.Exception("Bad direction");
			}
		}

		public Voxel GetVoxelBasedOnPlan(Pos pos, Direction direction) {
			return GetVoxelBasedOnPlan(pos.x, pos.y, pos.z, direction);
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

		public int SizeXBaseOnPlan(Direction direction) {
			switch (direction) {
			case Direction.south:
				return voxels.GetLength(0);
			case Direction.north:
				return voxels.GetLength(0);
			case Direction.west:
				return voxels.GetLength(2);
			case Direction.east:
				return voxels.GetLength(2);
			case Direction.up:
				return voxels.GetLength(0);
			case Direction.down:
				return voxels.GetLength(0);
			default:
				throw new System.Exception("Bad direction");
			}
		}

		public int SizeYBaseOnPlan(Direction direction) {
			switch (direction) {
			case Direction.south:
				return voxels.GetLength(1);
			case Direction.north:
				return voxels.GetLength(1);
			case Direction.west:
				return voxels.GetLength(1);
			case Direction.east:
				return voxels.GetLength(1);
			case Direction.up:
				return voxels.GetLength(2);
			case Direction.down:
				return voxels.GetLength(2);
			default:
				throw new System.Exception("Bad direction");
			}
		}

		public int SizeZBaseOnPlan(Direction direction) {
			switch (direction) {
			case Direction.south:
				return voxels.GetLength(2);
			case Direction.north:
				return voxels.GetLength(2);
			case Direction.west:
				return voxels.GetLength(0);
			case Direction.east:
				return voxels.GetLength(0);
			case Direction.up:
				return voxels.GetLength(1);
			case Direction.down:
				return voxels.GetLength(1);
			default:
				throw new System.Exception("Bad direction");
			}
		}

	}
}
