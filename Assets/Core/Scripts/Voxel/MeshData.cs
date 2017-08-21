using UnityEngine;
using System.Collections.Generic;

namespace Warfest {
	public class MeshData {
		public List<Vector3> vertices = new List<Vector3>();
		public List<int> triangles = new List<int>();
		public List<Vector2> uv = new List<Vector2>();
		public List<Color> colors = new List<Color>();
	}
}