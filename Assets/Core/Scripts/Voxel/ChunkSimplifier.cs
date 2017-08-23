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

//			// North
//			int nbLayers = chunk.SizeZBasedOnPlan(Direction.north);
//
//			for (int i = 0; i < nbLayers; i++) {
//				BuildFace(meshData, chunk, Direction.north, i);
//			}
//
//			// South
//			nbLayers = chunk.SizeZBasedOnPlan(Direction.south);
//
//			for (int i = 0; i < nbLayers; i++) {
//				BuildFace(meshData, chunk, Direction.south, i);
//			}
//
//			// Up
//			nbLayers = chunk.SizeZBasedOnPlan(Direction.up);
//
//			for (int i = 0; i < nbLayers; i++) {
//				BuildFace(meshData, chunk, Direction.up, i);
//			}
//
//			// Down
//			nbLayers = chunk.SizeZBasedOnPlan(Direction.down);
//
//			for (int i = 0; i < nbLayers; i++) {
//				BuildFace(meshData, chunk, Direction.down, i);
//			}
//
//			// West
//			nbLayers = chunk.SizeZBasedOnPlan(Direction.west);
//
//			for (int i = 0; i < nbLayers; i++) {
//				BuildFace(meshData, chunk, Direction.west, i);
//			}
//
//			// East
//			nbLayers = chunk.SizeZBasedOnPlan(Direction.east);
//
//			for (int i = 0; i < nbLayers; i++) {
//				BuildFace(meshData, chunk, Direction.east, i);
//			}

//			Debug.Log("==== north ====");
//			BuildFace(meshData, chunk, Direction.north, 0);
//			Debug.Log("==== south ====");
//			BuildFace(meshData, chunk, Direction.south, 0);
//			Debug.Log("==== west ====");
//			BuildFace(meshData, chunk, Direction.west, 0);
//			Debug.Log("==== east ====");
//			BuildFace(meshData, chunk, Direction.east, 0);
//			Debug.Log("==== up ====");
//			BuildFace(meshData, chunk, Direction.up, 0);
//			Debug.Log("==== down ====");
//			BuildFace(meshData, chunk, Direction.down, 0);

			foreach (var dir in DirectionUtils.Directions) {
				int nbLayers = chunk.SizeZBasedOnPlan(dir);

				for (int i = 0; i < nbLayers; i++) {
					BuildFace(meshData, chunk, dir, i);
				}
			}

			return meshData;
		}

		void BuildFace(MeshData meshData, Chunk chunk, Direction dir, int z) {
			HashSet<Vector2> usedPos = new HashSet<Vector2>();
			List<VoxelRect> rectangles = new List<VoxelRect>();

			int lineSize = 0;
			int compatibleLines = 0;
			Vector2 pos = new Vector2(0f, 0f);

			pos = GetNextPos(usedPos, pos, z, chunk, dir);
			while (pos != endPos) {
				Voxel currentVoxel = chunk.GetVoxelBasedOnPlan((int)pos.x, (int)pos.y, z, dir);

				lineSize = GetSimilarVoxelCountNextToThisPos(pos, z, currentVoxel.color, chunk, usedPos, dir);

				compatibleLines = GetCompatibleLines(pos, z, currentVoxel.color, chunk, lineSize, usedPos, dir);

				VoxelRect rect = new VoxelRect(pos.x, pos.y, z, lineSize, compatibleLines);
				rectangles.Add(rect);

				SetUsedPos(usedPos, rect);

				Debug.LogFormat("pos: {0}, lineSize: {1}, compatibleLines: {2}", pos, lineSize, compatibleLines);

				pos = GetNextPos(usedPos, pos, z, chunk, dir);
			}

			BuildRectangleMeshed(rectangles, meshData, chunk, dir);
		}

		Vector2 GetNextPos(HashSet<Vector2> usedPos, Vector2 pos, int z, Chunk chunk, Direction dir) {
			int x = (int)pos.x;

			bool isAlreadyUsed;
			bool isSolidVoxel;
			bool isFaceVisible;

			int chunkSizeX = chunk.SizeXBasedOnPlan(dir);
			int chunkSizeY = chunk.SizeYBasedOnPlan(dir);

			for (int y = (int)pos.y; y < chunkSizeY; y++) {
				for (; x < chunkSizeX; x++) {
					Vector2 currentPos = new Vector2(x, y);

					isAlreadyUsed = usedPos.Contains(currentPos);
					isSolidVoxel = chunk.GetVoxelBasedOnPlan(x, y, z, dir).IsSolid;
					isFaceVisible = IsFaceVisible(x, y, z, chunk, dir);

					if (!isAlreadyUsed && isSolidVoxel && isFaceVisible) {
						return currentPos;
					}
				}

				x = 0;
			}

			return endPos;
		}

		bool IsFaceVisible(int x, int y, int z, Chunk chunk, Direction dir) {
			if (z == 0) {
				return true;
			}

			return chunk.GetVoxelBasedOnPlan(x, y, z - 1, dir).IsAir;
		}

		int GetSimilarVoxelCountNextToThisPos(Vector2 pos, int z, Color32 color, Chunk chunk, HashSet<Vector2> usedPos, Direction dir) {
			int count = 1;
			int chunkSizeX = chunk.SizeXBasedOnPlan(dir);

			bool isAlreadyUsed;
			bool isSameColor;
			bool isFaceVisible;

			for (int x = (int)pos.x + 1; x < chunkSizeX; x++) {
				isAlreadyUsed = usedPos.Contains(new Vector2(x, pos.y));
				isSameColor = chunk.GetVoxelBasedOnPlan(x, (int)pos.y, z, dir).color.Equals(color);
				isFaceVisible = IsFaceVisible(x, (int)pos.y, z, chunk, dir);

				if (isAlreadyUsed || !isSameColor || !isFaceVisible) {
					return count;
				}

				count++;
			}

			return count;
		}

		bool IsLineCompatible(Vector2 pos, int z, int lineSize, Color32 color, Chunk chunk, HashSet<Vector2> usedPos, Direction dir) {
			int count = 0;
			int chunkSizeX = chunk.SizeXBasedOnPlan(dir);

			bool isAlreadyUsed;
			bool isSameColor;
			bool isFaceVisible;

			for (int x = (int)pos.x; x < chunkSizeX && count < lineSize; x++) {
				isAlreadyUsed = usedPos.Contains(new Vector2(x, pos.y));
				isSameColor = chunk.GetVoxelBasedOnPlan(x, (int)pos.y, z, dir).color.Equals(color);
				isFaceVisible = IsFaceVisible(x, (int)pos.y, z, chunk, dir);

				if (!isAlreadyUsed && isSameColor && isFaceVisible) {
					count++;
				} else {
					return false;
				}
			}

			return count == lineSize;
		}

		int GetCompatibleLines(Vector2 pos, int z, Color32 color, Chunk chunk, int lineSize, HashSet<Vector2> usedPos, Direction dir) {
			int count = 1;

			int chunkSizeY = chunk.SizeYBasedOnPlan(dir);

			for (int y = (int)pos.y + 1; y < chunkSizeY; y++) {
				if (!IsLineCompatible(new Vector2(pos.x, y), z, lineSize, color, chunk, usedPos, dir)) {
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
			if (originalDir == Direction.south) {
				return rect;
			}

			int chunkSizeX = chunk.SizeXBasedOnPlan(originalDir);
			int chunkSizeY = chunk.SizeYBasedOnPlan(originalDir);
			int chunkSizeZ = chunk.SizeZBasedOnPlan(originalDir);

			switch (originalDir) {
			case Direction.north:
				return new VoxelRect(
					chunkSizeX - rect.x - rect.width,
					rect.y,
					chunkSizeZ - rect.z - 1,
					rect.width,
					rect.height
				);
			case Direction.west:
				return new VoxelRect(
					chunkSizeX - rect.x - rect.width,
					rect.y,
					rect.z,
					rect.width,
					rect.height
				);
			case Direction.east:
				return new VoxelRect(
					rect.x,
					rect.y,
					chunkSizeZ - rect.z - 1,
					rect.width,
					rect.height
				);
			case Direction.up:
				return new VoxelRect(
					rect.x,
					rect.y,
					chunkSizeZ - rect.z - 1,
					rect.width,
					rect.height
				);
			case Direction.down:
				return new VoxelRect(
					rect.x,
					chunkSizeY - rect.y - rect.height,
					rect.z,
					rect.width,
					rect.height
				);
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

			switch (dir) {
			case Direction.north:
				vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, z + 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, endY   + 0.5f, z + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, z + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, z + 0.5f));
				break;
			case Direction.south:
				vertices.Add(new Vector3(startX - 0.5f, startY - 0.5f, z - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, endY   + 0.5f, z - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, endY   + 0.5f, z - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, startY - 0.5f, z - 0.5f));
				break;
			case Direction.west:
				vertices.Add(new Vector3(z - 0.5f, startY - 0.5f, endX + 0.5f));
				vertices.Add(new Vector3(z - 0.5f, endY   + 0.5f, endX + 0.5f));
				vertices.Add(new Vector3(z - 0.5f, endY   + 0.5f, startX - 0.5f));
				vertices.Add(new Vector3(z - 0.5f, startY - 0.5f, startX - 0.5f));
				break;
			case Direction.east:
				vertices.Add(new Vector3(z + 0.5f, startY - 0.5f, startX - 0.5f));
				vertices.Add(new Vector3(z + 0.5f, endY   + 0.5f, startX - 0.5f));
				vertices.Add(new Vector3(z + 0.5f, endY   + 0.5f, endX + 0.5f));
				vertices.Add(new Vector3(z + 0.5f, startY - 0.5f, endX + 0.5f));
				break;
			case Direction.up:
				vertices.Add(new Vector3(startX - 0.5f, z + 0.5f, endY   + 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, z + 0.5f, endY   + 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, z + 0.5f, startY - 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, z + 0.5f, startY - 0.5f));
				break;
			case Direction.down:
				vertices.Add(new Vector3(startX - 0.5f, z - 0.5f, startY - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, z - 0.5f, startY - 0.5f));
				vertices.Add(new Vector3(endX   + 0.5f, z - 0.5f, endY   + 0.5f));
				vertices.Add(new Vector3(startX - 0.5f, z - 0.5f, endY   + 0.5f));
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