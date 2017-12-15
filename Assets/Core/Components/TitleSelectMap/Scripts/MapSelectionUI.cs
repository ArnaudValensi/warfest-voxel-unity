using UnityEngine;

public class MapSelectionUI : MonoBehaviour {

	public enum Tab {
		MyWorlds = 0,
		Community
	}

	public Transform mapsHolderTransform;
	public MapSelectionTabSwitcher tabSwitcher;

	[ReadOnly] public Tab currentTab = Tab.MyWorlds;

	MapInfo[] mapInfos;

	void Start() {
		mapInfos = new MapInfo[] {
			new MapInfo("Unicorn island", "...", 32),
			new MapInfo("Sky land", "...", 32),
			new MapInfo("Nyan Dog", "...", 32),
			new MapInfo("The wall paper", "...", 32)
		};
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
		public string description;
		public int size;

		public MapInfo(string name, string description, int size) {
			this.name = name;
			this.description = description;
			this.size = size;
		}
	}

}
