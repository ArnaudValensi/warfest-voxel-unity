using UnityEngine;

public class MapSelectionUI : MonoBehaviour {

	public enum Tab {
		MyWorlds = 0,
		Community
	}

	public Transform mapsHolderTransform;
	public MapSelectionTabSwitcher tabSwitcher;
	public Transform mapsHolder;
	public GameObject mapPrefab;

	[ReadOnly] public Tab currentTab = Tab.MyWorlds;

	MapInfo[] mapInfos;

	void Start() {
		mapInfos = new [] {
			new MapInfo("Unicorn island", "9/11/17 20:35 PM"),
			new MapInfo("Sky land", "9/11/17 20:35 PM"),
			new MapInfo("Nyan Dog", "9/11/17 20:35 PM"),
			new MapInfo("The wall paper", "9/11/17 20:35 PM")
		};

		foreach (var mapInfo in mapInfos) {
			GameObject newMap = Instantiate(mapPrefab, mapsHolder);
			MapSelectionButton mapSelectionButton = newMap.GetComponent<MapSelectionButton>();

			mapSelectionButton.Init(mapInfo.name, mapInfo.date);
		}
	}

	// Tab Switch
	public void OnMyWorldsClicked() {
		SwitchToTab(Tab.MyWorlds);
	}

	public void OnCommunityClicked() {
		SwitchToTab(Tab.Community);
	}

	void SwitchToTab(Tab tab) {
		// Switch tab icon
		tabSwitcher.SwitchToTab(tab);

		currentTab = tab;
	}

	// On slot clicked
	public void OnMapClicked() {
		Debug.Log("OnMapClicked");

		// TODO
	}

	public class MapInfo {
		public string name;
		public string date;

		public MapInfo(string name, string date) {
			this.name = name;
			this.date = date;
		}
	}

}
