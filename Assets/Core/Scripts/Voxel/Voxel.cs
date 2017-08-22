using UnityEngine;

namespace Warfest {
	public class Voxel {

		public enum Type {
			Air,
			Solid,
		}

		public Type type;
		public bool IsAir { get { return type == Type.Air; } }
		public bool IsSolid { get { return !IsAir; } }

		public Vector2[] colorUvs;

		public Voxel(Type type, Vector2[] colorUvs) {
			this.type = type;
			this.colorUvs = colorUvs;
		}

	}
}