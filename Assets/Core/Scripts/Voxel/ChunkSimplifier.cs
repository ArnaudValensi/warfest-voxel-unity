using UnityEngine;
using System.Collections.Generic;

namespace Warfest {
	public class ChunkSimplifier : MonoBehaviour {

		readonly Vector2 endPos = new Vector2(-1, -1);

		ColorTexture colorTexture;

		void Start() {
			colorTexture = GameManager.Instance.GetColorTexture();
		}

		public MeshData BuildMesh(Chunk chunk) {
			MeshData meshData = new MeshData();

			BuildFace(meshData, chunk, Direction.south, 0);
			BuildFace(meshData, chunk, Direction.north, 0);
			BuildFace(meshData, chunk, Direction.west, 0);
			BuildFace(meshData, chunk, Direction.east, 0);
			BuildFace(meshData, chunk, Direction.up, 0);
			BuildFace(meshData, chunk, Direction.down, 0);

			return meshData;
		}

		void BuildFace(MeshData meshData, Chunk chunk, Direction dir, int z) {
			HashSet<Vector2> usedPos = new HashSet<Vector2>();
			List<VoxelRect> rectangles = new List<VoxelRect>();

			int lineSize = 0;
			int compatibleLines = 0;
			Vector2 pos = new Vector2(0f, 0f);

			pos = GetNextPos(usedPos, pos, chunk, dir);
			while (pos != endPos) {
				Voxel currentVoxel = chunk.GetVoxelBasedOnPlan((int)pos.x, (int)pos.y, z, dir);

				lineSize = GetSimilarVoxelCountNextToThisPos(pos, currentVoxel.color, chunk, usedPos, dir);

				compatibleLines = GetCompatibleLines(pos, currentVoxel.color, chunk, lineSize, usedPos, dir);

				VoxelRect rect = new VoxelRect(pos.x, pos.y, z, lineSize, compatibleLines);
				rectangles.Add(rect);

				SetUsedPos(usedPos, rect);

				Debug.LogFormat("pos: {0}, lineSize: {1}, compatibleLines: {2}", pos, lineSize, compatibleLines);

				pos = GetNextPos(usedPos, pos, chunk, dir);
			}

			BuildRectangleMeshed(rectangles, meshData, chunk, dir);
		}

		Vector2 GetNextPos(HashSet<Vector2> usedPos, Vector2 pos, Chunk chunk, Direction dir) {
			int x = (int)pos.x;

			int chunkSizeX = chunk.SizeXBasedOnPlan(dir);
			int chunkSizeY = chunk.SizeYBasedOnPlan(dir);

			for (int y = (int)pos.y; y < chunkSizeY; y++) {
				for (; x < chunkSizeX; x++) {
					Vector2 currentPos = new Vector2(x, y);

					if (!usedPos.Contains(currentPos) && chunk.GetVoxelBasedOnPlan(x, y, 0, dir).IsSolid) {
						return currentPos;
					}
				}

				x = 0;
			}

			return endPos;
		}

		int GetSimilarVoxelCountNextToThisPos(Vector2 pos, Color32 color, Chunk chunk, HashSet<Vector2> usedPos, Direction dir) {
			int count = 1;

			int chunkSizeX = chunk.SizeXBasedOnPlan(dir);

			for (int x = (int)pos.x + 1; x < chunkSizeX; x++) {
				if (usedPos.Contains(new Vector2(x, pos.y)) || !chunk.GetVoxelBasedOnPlan(x, (int)pos.y, 0, dir).color.Equals(color)) {
					return count;
				}

				count++;
			}

			return count;
		}

		bool IsLineCompatible(Vector2 pos, int lineSize, Color32 color, Chunk chunk, HashSet<Vector2> usedPos, Direction dir) {
			int count = 0;

			int chunkSizeX = chunk.SizeXBasedOnPlan(dir);

			for (int x = (int)pos.x; x < chunkSizeX && count < lineSize; x++) {
				if (!usedPos.Contains(new Vector2(x, pos.y)) && chunk.GetVoxelBasedOnPlan(x, (int)pos.y, 0, dir).color.Equals(color)) {
					count++;
				} else {
					return false;
				}
			}

			return count == lineSize;
		}

		int GetCompatibleLines(Vector2 pos, Color32 color, Chunk chunk, int lineSize, HashSet<Vector2> usedPos, Direction dir) {
			int count = 1;

			int chunkSizeY = chunk.SizeYBasedOnPlan(dir);

			for (int y = (int)pos.y + 1; y < chunkSizeY; y++) {
				if (!IsLineCompatible(new Vector2(pos.x, y), lineSize, color, chunk, usedPos, dir)) {
					return count;
				}

				count++;
			}

			return count;
		}

		void SetUsedPos(HashSet<Vector2> usedPos, VoxelRect rect) {
			for (int y = (int)rect.y; y < (int)rect.y + (int)rect.height; y++) {
				for (int x = (int)rect.x; x < (int)rect.x + (int)rect.width; x++) {
					usedPos.Add(
						new Vector2(x, y)
					);
				}
			}
		}

		VoxelRect GetRegularPlanRect(VoxelRect rect, Direction originalDir, Chunk chunk) {
			float x;
			float y;
			float z;
			float width;
			float height;

			if (originalDir == Direction.south) {
				return rect;
			}

			int chunkSizeX = chunk.SizeXBasedOnPlan(originalDir);
			int chunkSizeY = chunk.SizeYBasedOnPlan(originalDir);
			int chunkSizeZ = chunk.SizeZBasedOnPlan(originalDir);

			switch (originalDir) {
			case Direction.north:
				x = chunkSizeX - rect.x - rect.width;
				y = rect.y;
				z = rect.z;
				width = rect.width;
				height = rect.height;

				return new VoxelRect(x, y, z, width, height);
			case Direction.west:
				return rect;
			case Direction.east:
				Debug.Log("chunkSizeZ: " + chunkSizeZ + ", rect.z: " + rect.z);
				return new VoxelRect(rect.x, rect.y, chunkSizeZ - rect.z - 1, rect.width, rect.height);
			case Direction.up:
				return new VoxelRect(rect.x, rect.y, chunkSizeZ - rect.z - 1, rect.width, rect.height);
			case Direction.down:
				return rect;
			default:
				throw new System.Exception("Bad direction");
			}
		}

		void BuildRectangleMeshed(List<VoxelRect> rectangles, MeshData meshData, Chunk chunk, Direction dir) {
			foreach (var rect in rectangles) {
				VoxelRect regularPlanRect = GetRegularPlanRect(rect, dir, chunk);

				CreateVertexFace(meshData, regularPlanRect, dir);
				AddQuadTriangles(meshData);

//				Vector2 colorUv = colorTexture.GetColorUV(
//					chunk.GetVoxel((int)rect.x, (int)rect.y, 0).color
//				);
//
//				meshData.uv.Add(colorUv);
//				meshData.uv.Add(colorUv);
//				meshData.uv.Add(colorUv);
//				meshData.uv.Add(colorUv);
			}
		}

		void CreateVertexFace(MeshData meshData, VoxelRect rect, Direction dir) {
			var vertices = meshData.vertices;
			float startX = rect.x;
			float startY = rect.y;
			float endX = rect.x + rect.width - 1;
			float endY = rect.y + rect.height - 1;
			float z = rect.z;

//			vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f - 0.5f));
//			vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, 0f - 0.5f));
//			vertices.Add(new Vector3(endX   + 0.5f, endY   + 0.5f, 0f - 0.5f));
//			vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, 0f - 0.5f));

			switch (dir) {
			case Direction.north:
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, endY   + 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f + 0.5f));
				break;
			case Direction.east:
				Debug.Log("z: " + z);
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(z + endX + 0.5f, startY - 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(z + endX + 0.5f, endY   + 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(z + endX + 0.5f, endY   + 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(z + endX + 0.5f, startY - 0.5f, 0f + 0.5f));
				break;
			case Direction.south:
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, endY   + 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, 0f - 0.5f));
				break;
			case Direction.west:
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f - 0.5f));
				break;
			case Direction.up:
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, z + endY + 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, z + endY + 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, z + endY + 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, z + endY + 0.5f, 0f - 0.5f));
				break;
			case Direction.down:
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z - 0.5f));
//				vertices.Add(new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z + 0.5f));
//				vertices.Add(new Vector3(pos.x - 0.5f, pos.y - 0.5f, pos.z + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, 0f - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, 0f + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f + 0.5f));
				break;
			default:
				throw new System.Exception("Bad direction");
			}
		}

		void AddQuadTriangles(MeshData meshData) {
			var triangles = meshData.triangles;
			var vertices = meshData.vertices;

			triangles.Add(vertices.Count - 4);
			triangles.Add(vertices.Count - 3);
			triangles.Add(vertices.Count - 2);

			triangles.Add(vertices.Count - 4);
			triangles.Add(vertices.Count - 2);
			triangles.Add(vertices.Count - 1);
		}

		public Mesh RenderMesh(MeshData meshData, MeshFilter filter, MeshCollider coll) {
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

		public class VoxelRect {
			public float x;
			public float y;
			public float z;
			public float width;
			public float height;

			public VoxelRect(float x, float y, float z, float width, float height) {
				this.x = x;
				this.y = y;
				this.z = z;
				this.width = width;
				this.height = height;
			}
		}

	}
}