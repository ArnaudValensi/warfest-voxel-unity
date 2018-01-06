using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SimpleJSON;

public class WorldsManager : MonoBehaviour {

	public List<WorldInfo> GetWorldList() {
		string savePath = GameConfig.Instance.GetSavePath();
		List<WorldInfo> worldList = new List<WorldInfo>();

		foreach (string folder in Directory.GetDirectories(savePath)) {
			string worldJsonPath = folder + "/world.json";

			if (File.Exists(worldJsonPath)) {
				string worldData = File.ReadAllText(worldJsonPath);
				var worldJson = JSON.Parse(worldData);

				WorldInfo worldInfo = new WorldInfo(worldJson["name"], worldJson["date"]);
				worldList.Add(worldInfo);
			}
		}

		return worldList;
	}

	bool isWorldExists() {
		List<WorldInfo> worlds = GetWorldList();

		return worlds.FirstOrDefault(world => world.name == name) != null;
	}

	public void CreateWorld(string name) {
		if (isWorldExists()) {
			throw new UnityException("A map with the same name already exists");
		}
	}

	public void DeteleWorld() {
		throw new UnityException("Not implmented yet");
	}

	public void LoadWorld() {
		throw new UnityException("Not implmented yet");
	}

}
