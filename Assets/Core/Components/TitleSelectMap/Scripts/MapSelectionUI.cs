using UnityEngine;

public class MapSelectionUI : MonoBehaviour {

	public enum Tab {
		MyWorlds = 0,
		Community
	}

	public Transform mapsHolderTransform;
	public MapSelectionTabSwitcher tabSwitcher;

	[ReadOnly] public Tab currentTab = Tab.MyWorlds;

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

}
