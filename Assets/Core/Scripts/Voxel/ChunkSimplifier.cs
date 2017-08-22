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

			BuildFace(meshData, chunk, Direction.south);

			return meshData;
		}

		void BuildFace(MeshData meshData, Chunk chunk, Direction dir) {
			HashSet<Vector2> usedPos = new HashSet<Vector2>();
			List<VoxelRect> rectangles = new List<VoxelRect>();

			int lineSize = 0;
			int compatibleLines = 0;
			Vector2 pos = new Vector2(0f, 0f);

			pos = GetNextPos(usedPos, pos, chunk, dir);
			while (pos != endPos) {
				Voxel currentVoxel = chunk.GetVoxelBasedOnPlan((int)pos.x, (int)pos.y, 0, dir);

				lineSize = GetSimilarVoxelCountNextToThisPos(pos, currentVoxel.color, chunk, usedPos, dir);

				compatibleLines = GetCompatibleLines(pos, currentVoxel.color, chunk, lineSize, usedPos, dir);

				VoxelRect rect = new VoxelRect(pos.x, pos.y, lineSize, compatibleLines);
				rectangles.Add(rect);

				SetUsedPos(usedPos, rect);

				Debug.LogFormat("pos: {0}, lineSize: {1}, compatibleLines: {2}", pos, lineSize, compatibleLines);

				pos = GetNextPos(usedPos, pos, chunk, dir);
			}

			BuildRectangleMeshed(rectangles, meshData, chunk);
		}

		Vector2 GetNextPos(HashSet<Vector2> usedPos, Vector2 pos, Chunk chunk, Direction dir) {
			int x = (int)pos.x;

			for (int y = (int)pos.y; y < chunk.SizeY; y++) {
				for (; x < chunk.SizeX; x++) {
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

			for (int x = (int)pos.x + 1; x < chunk.SizeX; x++) {
				if (usedPos.Contains(new Vector2(x, pos.y)) || !chunk.GetVoxelBasedOnPlan(x, (int)pos.y, 0, dir).color.Equals(color)) {
					return count;
				}

				count++;
			}

			return count;
		}

		bool IsLineCompatible(Vector2 pos, int lineSize, Color32 color, Chunk chunk, HashSet<Vector2> usedPos, Direction dir) {
			int count = 0;

			for (int x = (int)pos.x; x < chunk.SizeX && count < lineSize; x++) {
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

			for (int y = (int)pos.y + 1; y < chunk.SizeY; y++) {
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

		void BuildRectangleMeshed(List<VoxelRect> rectangles, MeshData meshData, Chunk chunk) {
			foreach (var rect in rectangles) {
				CreateVertexFace(meshData, rect);
				AddQuadTriangles(meshData);

				Vector2 colorUv = colorTexture.GetColorUV(
					chunk.GetVoxel((int)rect.x, (int)rect.y, 0).color
				);

				meshData.uv.Add(colorUv);
				meshData.uv.Add(colorUv);
				meshData.uv.Add(colorUv);
				meshData.uv.Add(colorUv);
			}
		}

		void CreateVertexFace(MeshData meshData, VoxelRect rect) {
			var vertices = meshData.vertices;
			float startX = rect.x;
			float startY = rect.y;
			float endX = rect.x + rect.width - 1;
			float endY = rect.y + rect.height - 1;

			vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, 0f - 0.5f));
			vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, 0f - 0.5f));
			vertices.Add(new Vector3(endX   + 0.5f, endY   + 0.5f, 0f - 0.5f));
			vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, 0f - 0.5f));
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
			public float width;
			public float height;

			public VoxelRect(float x, float y, float width, float height) {
				this.x = x;
				this.y = y;
				this.width = width;
				this.height = height;
			}
		}

	}
}