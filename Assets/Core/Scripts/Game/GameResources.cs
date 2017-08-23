using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Warfest {
	[ExecuteInEditMode]
	public class GameResources : MonoBehaviour {

		Dictionary<string, Chunk> models;

		void OnEnable() {
			Debug.Log("[GameResources] OnEnable");
		}

		void Awake() {
			Debug.Log("[GameResources] Awake");

			models = new Dictionary<string, Chunk>();

			LoadQbtModels();
		}

		void LoadQbtModels() {
			string qbtPath = GameConfig.Instance.GetModelsPath();
			ColorTexture colorTexture = GameObject.Find("/Managers/ColorTexture").GetComponent<ColorTexture>();

			foreach (string file in Directory.GetFiles(qbtPath, "*.qbt")) {
				string filename = Path.GetFileName(file);

				Debug.Log("[GameResources] Loading " + filename);

				QBTFile qbtFile = new QBTFile(file);
				QBTFile.VoxelData[,,] qbtData = qbtFile.VoxelsData;

				colorTexture.AddColors(qbtFile.Colors.ToArray());

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
									x, y, sizeZ - 1 -z,
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

	}
}
