using UnityEngine;

namespace Warfest {
	public static class VoxelMeshBuilder {

		public static MeshData BuildMesh(Chunk chunk) {
			MeshData meshData = new MeshData();

			for (int x = 0; x < chunk.SizeX; x++) {
				for (int y = 0; y < chunk.SizeY; y++) {
					for (int z = 0; z < chunk.SizeZ; z++) {
						if (chunk.voxels[x, y, z].IsSolid) { // If it is solid
							meshData = Voxeldata(chunk, x, y, z, meshData);
						}
					}
				}
			}

			return meshData;
		}

		static MeshData Voxeldata(Chunk chunk, int x, int y, int z, MeshData meshData) {
			Pos pos = new Pos(x, y, z);

			foreach (var dir in DirectionUtils.Directions) {
				Pos touchingPos = pos + dir;

				// Build the face if there is no touching cube or if is the side of the chunk.
				if (chunk.IsOutOfRange(touchingPos) || chunk.GetVoxel(touchingPos).IsAir) {
					VoxelGeometry.CreateVertexFace(meshData, pos, dir);
					VoxelGeometry.AddQuadTriangles(meshData);
				}
			}

			return meshData;
		}

		public static Mesh RenderMesh(MeshData meshData, MeshFilter filter, MeshCollider coll) {
			MeshData data = (MeshData)meshData;

			filter.mesh.Clear();
			filter.mesh.vertices = data.vertices.ToArray();
			filter.mesh.triangles = data.triangles.ToArray();

			filter.mesh.uv = data.uv.ToArray();
			filter.mesh.RecalculateNormals();

			coll.sharedMesh = null;
			Mesh mesh = new Mesh();
			mesh.vertices = data.vertices.ToArray();
			mesh.triangles = data.triangles.ToArray();
			mesh.RecalculateNormals();

			coll.sharedMesh = mesh;

			return filter.mesh;
		}

	}
}