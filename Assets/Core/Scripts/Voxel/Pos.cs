using UnityEngine;
using System;

namespace Warfest {
	[Serializable]
	public struct Pos {
		public int x, y, z;

		public Pos(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 47;
				hash = hash * 227 + x.GetHashCode();
				hash = hash * 227 + y.GetHashCode();
				hash = hash * 227 + z.GetHashCode();
				return hash;
			}
		}

		public override bool Equals(object obj) {
			if (GetHashCode() == obj.GetHashCode())
				return true;
			return false;
		}

		//Pos and Vector3 can be substituted for one another
		public static implicit operator Pos(Vector3 v) {
			return new Pos((int)v.x, (int)v.y, (int)v.z);
		}

		public static implicit operator Vector3(Pos pos) {
			return new Vector3(pos.x, pos.y, pos.z);
		}

		public Pos Add(int x, int y, int z) {
			return new Pos(this.x + x, this.y + y, this.z + z);
		}

		public Pos Add(Pos pos) {
			return new Pos(this.x + pos.x, this.y + pos.y, this.z + pos.z);
		}

		public Pos Subtract(Pos pos) {
			return new Pos(this.x - pos.x, this.y - pos.y, this.z - pos.z);
		}

		//These operators let you add and subtract Pos from each other
		//or check equality with == and !=
		public static Pos operator -(Pos pos1, Pos pos2) {
			return pos1.Subtract(pos2);
		}

		public static Pos operator +(Pos pos1, Pos pos2) {
			return pos1.Add(pos2);
		}

		public static bool operator ==(Pos pos1, Pos pos2) {
			return Equals(pos1, pos2);
		}

		public static bool operator !=(Pos pos1, Pos pos2) {
			return !Equals(pos1, pos2);
		}

		//You can safely use Pos as part of a string like this:
		//"voxel at " + Pos + " is broken."
		public override string ToString() {
			return "(" + x + ", " + y + ", " + z + ")";
		}

		public static implicit operator Pos(Direction dir) {
			switch (dir) {
			case Direction.north:
				return new Pos(0, 0, 1);
			case Direction.east:
				return new Pos(1, 0, 0);
			case Direction.south:
				return new Pos(0, 0, -1);
			case Direction.west:
				return new Pos(-1, 0, 0);
			case Direction.up:
				return new Pos(0, 1, 0);
			case Direction.down:
				return new Pos(0, -1, 0);
			default:
				return new Pos(0, 0, 1);
			}
		}

		//returns the position of the chunk containing this block
		public Pos ContainingChunkCoordinates(int chunkSize) {
			int x = Mathf.FloorToInt(this.x / (float)chunkSize) * chunkSize;
			int y = Mathf.FloorToInt(this.y / (float)chunkSize) * chunkSize;
			int z = Mathf.FloorToInt(this.z / (float)chunkSize) * chunkSize;

			return new Pos(x, y, z);
		}

		public Pos ToLocalChunkCoordinates(int chunkSize) {
			int x = this.x - (Mathf.FloorToInt(this.x / (float)chunkSize) * chunkSize);
			int y = this.y - (Mathf.FloorToInt(this.y / (float)chunkSize) * chunkSize);
			int z = this.z - (Mathf.FloorToInt(this.z / (float)chunkSize) * chunkSize);

			return new Pos(x, y, z);
		}

	}
}