using UnityEngine;

namespace Warfest {
	public static class VoxelGeometry {

		public static void CreateVertexFace(MeshData meshData, Pos pos, Direction dir) {
			var vertices = meshData.vertices;

			switch (dir) {
			case Direction.north:
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				break;
			case Direction.east:
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				break;
			case Direction.south:
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				break;
			case Direction.west:
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				break;
			case Direction.up:
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z - 0.5f));
				break;
			case Direction.down:
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				break;
			default:
				throw new System.Exception("Bad direction");
			}
		}

		public static void AddQuadTriangles(MeshData meshData) {
			var triangles = meshData.triangles;
			var vertices = meshData.vertices;

			triangles.Add(vertices.Count - 4);
			triangles.Add(vertices.Count - 3);
			triangles.Add(vertices.Count - 2);

			triangles.Add(vertices.Count - 4);
			triangles.Add(vertices.Count - 2);
			triangles.Add(vertices.Count - 1);
		}

	}
}