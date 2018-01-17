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
				string worldFolderName = Path.GetFileName(folder);

				WorldInfo worldInfo = new WorldInfo(worldJson["name"], worldFolderName, worldJson["date"]);
				worldList.Add(worldInfo);
			}
		}

		worldList.Sort((world1, world2) => DateUtil.CompareStringDate(world2.date, world1.date));

		return worldList;
	}

	bool isWorldExists(string worldName) {
		List<WorldInfo> worlds = GetWorldList();

		return worlds.FirstOrDefault(world => world.name == worldName) != null;
	}

	public void CreateWorld(string worldName) {
		if (string.IsNullOrEmpty(worldName)) {
			throw new UnityException("A name must be specified to create a world");
		}

		string worldPath = GetWorldFolderNameToCreate(worldName);
		string worldJsonPath = worldPath + "/world.json";
		JSONNode worldJson = new JSONObject();

		worldJson["name"] = worldName;
		worldJson["date"] = DateUtil.GetCurrentStringDate();

		Directory.CreateDirectory(worldPath);
		File.WriteAllText(worldJsonPath, worldJson.ToString(2));
	}

	string GetWorldFolderNameToCreate(string worldName) {
		string savePath = GameConfig.Instance.GetSavePath();
		string worldPath = savePath + "/" + worldName;

		while (Directory.Exists(worldPath)) {
			worldPath += "-";
		}

		return worldPath;
	}

	string GetWorldPathFromWorldInfo(WorldInfo worldInfo) {
		string savePath = GameConfig.Instance.GetSavePath();

		return savePath + "/" + worldInfo.folderName;
	}

	public void DeteleWorld(WorldInfo worldInfo) {
		string worldPath = GetWorldPathFromWorldInfo(worldInfo);

		Directory.Delete(worldPath, true);
	}

	public void LoadWorld() {
		throw new UnityException("Not implmented yet");
	}

}
