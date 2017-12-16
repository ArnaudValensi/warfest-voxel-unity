using UnityEngine;
using System;

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
	MapSelectionButton[] mapSelectionButtons;
	int selectedMap = -1;

	void Start() {
		mapInfos = new [] {
			new MapInfo("Unicorn island", "9/11/17 20:35 PM"),
			new MapInfo("Sky land", "9/11/17 20:35 PM"),
			new MapInfo("Nyan Dog", "9/11/17 20:35 PM"),
			new MapInfo("The wall paper", "9/11/17 20:35 PM")
		};

		mapSelectionButtons = new MapSelectionButton[mapInfos.Length];

		int i = 0;
		foreach (var mapInfo in mapInfos) {
			GameObject newMap = Instantiate(mapPrefab, mapsHolder);
			MapSelectionButton mapSelectionButton = newMap.GetComponent<MapSelectionButton>();

			mapSelectionButtons[i] = mapSelectionButton;
			mapSelectionButton.Init(mapInfo.name, mapInfo.date);

			Func<int, Action> CreateClickAction = (index) => () => SelectMap(index);

			mapSelectionButton.OnClicked += CreateClickAction(i);

			i++;
		}
	}

	void SelectMap(int index) {
		if (selectedMap != -1) {
			mapSelectionButtons[selectedMap].Unselect();
		}
		mapSelectionButtons[index].Select();
		selectedMap = index;
	}

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

	public class MapInfo {
		public string name;
		public string date;

		public MapInfo(string name, string date) {
			this.name = name;
			this.date = date;
		}
	}

}
