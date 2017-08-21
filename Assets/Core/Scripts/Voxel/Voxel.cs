namespace Warfest {
	public class Voxel {

		public enum Type {
			Air,
			Solid,
		}

		public Type type;
		public bool IsAir { get { return type == Type.Air; } }
		public bool IsSolid { get { return !IsAir; } }

		public Voxel(Type type = Type.Air) {
			this.type = type;
		}

	}
}