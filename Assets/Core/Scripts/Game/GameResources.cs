using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Warfest {
	[ExecuteInEditMode]
	public class GameResources : MonoBehaviour {

		[SerializeField]
		string paletteModel = null;

		ChunkSimplifier chunkSimplifier;
		Dictionary<string, Chunk> models;
		ColorTexture colorTexture;

		void OnEnable() {
			Debug.Log("[GameResources] OnEnable");

			chunkSimplifier = GameObject.Find("/Managers/ChunkSimplifier").GetComponent<ChunkSimplifier>();
			colorTexture = GameObject.Find("/Managers/ColorTexture").GetComponent<ColorTexture>();

			LoadTextureColor();
		}

		void Awake() {
			Debug.Log("[GameResources] Awake");

		}

		public void BakeTextureFromModel() {
			if (string.IsNullOrEmpty(paletteModel)) {
				Debug.Log("[GameResources] BakeTextureFromModel: paletteModel is not defined");
				return;
			}

			// Load qbt file
			string qbtPath = GameConfig.Instance.GetModelsPath() + "/" + paletteModel + ".qbt";
			QBTFile qbtFile = new QBTFile(qbtPath);

			// Create texture
			colorTexture.Init();
			colorTexture.AddColors(qbtFile.Colors.ToArray());

			// Save texture
			string texturePath = Application.dataPath + "/Core/VoxelModels/Textures/Voxel.png";
			File.WriteAllBytes(texturePath, colorTexture.Texture.EncodeToPNG());
			AssetDatabase.Refresh();

			Material material = (Material) (AssetDatabase.LoadAssetAtPath("Assets/Core/VoxelModels/Materials/Voxel.mat", typeof(Material)));

			if (material == null) {
				throw new UnityException("Null material");
			}

			material.mainTexture = (Texture2D) (AssetDatabase.LoadAssetAtPath("Assets/Core/VoxelModels/Textures/Voxel.png", typeof(Texture2D)));

			AssetDatabase.SaveAssets();

			Debug.Log(material.name);
		}

		void LoadTextureColor() {
			Texture2D bakedTexture = (Texture2D) (AssetDatabase.LoadAssetAtPath("Assets/Core/VoxelModels/Textures/Voxel.png", typeof(Texture2D)));

			if (bakedTexture == null) {
				Debug.Log("[GameResources] no baked texture to load");
				return;
			}

			colorTexture.Init();
			colorTexture.LoadTexture(bakedTexture);
		}

		void LoadQbtModels() {
			models = new Dictionary<string, Chunk>();

			string qbtPath = GameConfig.Instance.GetModelsPath();

			foreach (string file in Directory.GetFiles(qbtPath, "*.qbt")) {
				string filename = Path.GetFileNameWithoutExtension(file);

				QBTFile qbtFile = new QBTFile(file);
				QBTFile.VoxelData[,,] qbtData = qbtFile.VoxelsData;

				int sizeX = qbtData.GetLength(0);
				int sizeY = qbtData.GetLength(1);
				int sizeZ = qbtData.GetLength(2);

				Chunk chunk = new Chunk(sizeX, sizeY, sizeZ);
				for (int x = 0; x < sizeX; x++) {
					for (int y = 0; y < sizeY; y++) {
						for (int z = 0; z < sizeZ; z++) {
							if (qbtData[x, y, z].m != 0) {
								// The z axis is reversed in qubicle
								chunk.SetVoxel(
									x, y, sizeZ - 1 - z,
									Voxel.Type.Solid,
									qbtData[x, y, z].Color
								);
							}
						}
					}
				}

				models.Add(filename, chunk);
			}
		}

		public Chunk GetChunk(string modelName) {
			return models[modelName];
		}

		public string[] GetAvailableModelNames() {
			if (models == null) {
				return new string[0];
			}
			return models.Keys.ToArray();
		}

		public void GenerateModels() {
			Debug.Log("[GameResources] GeneratePrefab");

			LoadQbtModels();

			// Generate meshes
			foreach (var item in models) {
				string modelName = item.Key;
				Chunk chunk = item.Value;
				string destPath = "Assets/Core/VoxelModels/Meshes/" + modelName + ".asset";

				Debug.Log("[GameResources] Loading " + modelName);

				MeshData meshData = chunkSimplifier.BuildMesh(chunk);
				Mesh mesh = chunkSimplifier.CreateMesh(meshData);

				CreateOrReplaceAsset(mesh, destPath);
				AssetDatabase.SaveAssets();
			}
		}

		T CreateOrReplaceAsset<T>(T asset, string path) where T:Object {
			T existingAsset = AssetDatabase.LoadAssetAtPath<T>(path);

			if (existingAsset == null) {
				AssetDatabase.CreateAsset(asset, path);
				existingAsset = asset;
			} else {
				EditorUtility.CopySerialized(asset, existingAsset);
			}

			return existingAsset;
		}

	}
}
