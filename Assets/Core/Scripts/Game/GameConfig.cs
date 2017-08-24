using UnityEngine;

public class GameConfig : MonoBehaviourSingleton<GameConfig> {

	public string saveFolder = "save";

	public string GetSavePath() {
		return Application.persistentDataPath + "/" + saveFolder;
	}

	public string GetModelsPath() {
		return Application.dataPath + "/StreamingAssets/Models";
	}

	public string GetVoxelModelsPath() {
		return Application.dataPath + "/Core/Prefabs/VoxelModels";
	}

}
