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
		public Color32 color;

		public Voxel(Type type = Type.Air, Color32? color = null) {
			this.type = type;
			this.color = color ?? Color.black;
		}

	}
}