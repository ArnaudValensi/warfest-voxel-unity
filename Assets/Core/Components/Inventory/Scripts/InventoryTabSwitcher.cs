﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTabSwitcher : MonoBehaviour {

	public List<GameObject> tabIcons;
	public Color selectedTabIconColor;
	public Color unselectedTabIconColor;

	InventoryUI.Tab currentTab = InventoryUI.Tab.Ingredient;

	public void SwitchToTab(InventoryUI.Tab tab) {
		if (tab == currentTab) {
			return; // Pass
		}

		GameObject currentTabIcon = tabIcons[(int)currentTab];
		GameObject newTabIcon = tabIcons[(int)tab];

		// Icon
		currentTabIcon.GetComponent<Image>().color = unselectedTabIconColor;
		newTabIcon.GetComponent<Image>().color = selectedTabIconColor;

		// Title
		currentTabIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
		newTabIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;

		// Underline
		currentTabIcon.transform.GetChild(1).GetComponent<Image>().enabled = false;
		newTabIcon.transform.GetChild(1).GetComponent<Image>().enabled = true;

		currentTab = tab;
	}

}
