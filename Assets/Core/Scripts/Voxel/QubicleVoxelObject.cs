using UnityEngine;
using System.Linq;

namespace Warfest {
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class QubicleVoxelObject : MonoBehaviour {

		[ReadOnly][SerializeField] int sizeX;
		[ReadOnly][SerializeField] int sizeY;
		[ReadOnly][SerializeField] int sizeZ;

		Chunk chunk;
		MeshFilter meshFilter;
		MeshCollider meshCollider;
		MeshRenderer meshRenderer;
		ColorTexture colorTexture;
//		VoxelMeshBuilder voxelMeshBuilder;
		ChunkSimplifier chunkSimplifier;

		void Start() {
			meshFilter = GetComponent<MeshFilter>();
			meshCollider = GetComponent<MeshCollider>();
			meshRenderer = GetComponent<MeshRenderer>();
			colorTexture = GameManager.Instance.GetColorTexture();
//			voxelMeshBuilder = GameManager.Instance.GetVoxelMeshBuilder();
			chunkSimplifier = GameObject.Find("/Managers/ChunkSimplifier").GetComponent<ChunkSimplifier>();

			QBTFile qbtFile = LoadQubicleFile();
			QBTFile.VoxelData[,,] qbtData = qbtFile.VoxelsData;

			colorTexture.AddColors(qbtFile.Colors.ToArray());

			sizeX = qbtData.GetLength(0);
			sizeY = qbtData.GetLength(1);
			sizeZ = qbtData.GetLength(2);

			chunk = new Chunk(sizeX, sizeY, sizeZ);
			for (int x = 0; x < sizeX; x++) {
				for (int y = 0; y < sizeY; y++) {
					for (int z = 0; z < sizeZ; z++) {
						if (qbtData[x, y, z].m != 0) {
							// The z axis is reversed in qubicle
							chunk.SetVoxel(
								x, y, sizeZ - 1 -z,
								Voxel.Type.Solid,
								qbtData[x, y, z].Color
							);
						}
					}
				}
			}

			meshRenderer.sharedMaterial.mainTexture = colorTexture.Texture;

//			MeshData meshData = voxelMeshBuilder.BuildMesh(chunk);
//			voxelMeshBuilder.RenderMesh(meshData, meshFilter, meshCollider);

			MeshData meshData = chunkSimplifier.BuildMesh(chunk);
			chunkSimplifier.RenderMesh(meshData, meshFilter, meshCollider);
		}

		QBTFile LoadQubicleFile() {
			string qbtPath = GameConfig.Instance.GetModelsPath() + "/Deer.qbt";

			Debug.Log("Path: " + qbtPath);

			return new QBTFile(qbtPath);
		}

	}
}
