using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : MonoBehaviour {

	public enum Tab {
		Gun = 0,
		Clothe,
		Ingredient,
		Blueprint,
		Special
	}

	public Transform slotsHolderTransform;
	public InventoryTabSwitcher tabSwitcher;
	public GameObject infoPanel;
	// Text component to set selected item title
	public TextMeshProUGUI itemTitleText;
	// Text component to set selected item description
	public TextMeshProUGUI itemDescriptionText;
	// Action menu popup
	public GameObject actionMenu;
	// Action menu selection
	public Transform actionMenuSelectionTransform;

	GameObject inventoryPanelObj;
	//GameManager gameManager;
	bool isShown;
	MonoBehaviour blurScript;
	Inventory inventory;
	[ReadOnly] public Tab currentTab = Tab.Ingredient;
	int selectedSlot;
	List<Item> items;
	bool isActionMenuOpen;

	// UI Slots
	readonly List<GameObject> slots = new List<GameObject>();
	List<Image> slotImages = new List<Image>();

	void Start () {
		inventoryPanelObj = transform.Find("InventoryPanel").gameObject;
		blurScript = Camera.main.GetComponent<BlurOptimized>();
		//gameManager = GameObject.Find("/Managers").GetComponent<GameManager>();
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

		inventoryPanelObj.SetActive(false);

		// Get all slots
		foreach (Transform child in slotsHolderTransform) {
			slots.Add(child.gameObject);
			slotImages.Add(child.GetChild(0).GetComponent<Image>());
		}
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)) {
			if (isShown) {
				isShown = false;
				inventoryPanelObj.SetActive(false);
				blurScript.enabled = false;

				OpenActionMenu(false);

				//gameManager.ResumeGame();

				Warfest.MouseState.SetLocked(true);
			} else {
				ShowItems();
				RefreshSlotSelection();

				isShown = true;
				inventoryPanelObj.SetActive(true);
				blurScript.enabled = true;
			
				//gameManager.PauseGame();

				Warfest.MouseState.SetLocked(false);
			}
		}
	}

	void ShowItems() {
		items = GetItemsToShow();

		for (int i = 0; i < slots.Count; i++) {
			Image slotImage = slotImages[i];

			if (i < items.Count) {
				slotImage.enabled = true;
				slotImage.sprite = items[i].sprite;
			} else {
				slotImage.enabled = false;
			}
		}
	}

	List<Item> GetItemsToShow() {
		List<Item> items;

		switch (currentTab) {
		case Tab.Gun:
			items = inventory.GetItems(Item.Type.Gun);
			break;
		case Tab.Clothe:
			items = inventory.GetItems(Item.Type.Clothe);
			break;
		case Tab.Ingredient:
			items = inventory.GetItems(Item.Type.Ingredient);
			break;
		case Tab.Blueprint:
			items = inventory.GetItems(Item.Type.Blueprint);
			break;
		case Tab.Special:
			items = inventory.GetItems(Item.Type.Special);
			break;
		default:
			items = new List<Item>();
			break;
		}

		return items;
	}

	// Tab Switch
	public void OnGunClicked() {
		SwitchToTab(Tab.Gun);
	}

	public void OnClotheClicked() {
		SwitchToTab(Tab.Clothe);
	}

	public void OnIngredientClicked() {
		SwitchToTab(Tab.Ingredient);
	}

	public void OnBlueprintClicked() {
		SwitchToTab(Tab.Blueprint);
	}

	public void OnSpecialClicked() {
		SwitchToTab(Tab.Special);
	}

	void SwitchToTab(Tab tab) {
		if (isActionMenuOpen || tab == currentTab) {
			return; // Pass
		}

		// Switch tab icon
		tabSwitcher.SwitchToTab(tab);

		currentTab = tab;
		ShowItems();
		RefreshSlotSelection();
	}

	// On select slot hover
	public void SelectSlot(int index) {
		if (isActionMenuOpen) {
			return;
		}

		Transform slotSelectionTransform = slots[selectedSlot].transform.Find("SlotSelection");

		slotSelectionTransform.SetParent(slots[index].transform, false);
		selectedSlot = index;

		RefreshSlotSelection();
	}

	void RefreshSlotSelection() {
		if (selectedSlot < items.Count) {
			Item selectedItem = items[selectedSlot];

			itemTitleText.text = selectedItem.itemName;
			itemDescriptionText.text = selectedItem.itemDescription;

			if (!infoPanel.activeSelf) {
				infoPanel.SetActive(true);
			}
		} else {
			if (infoPanel.activeSelf) {
				infoPanel.SetActive(false);
			}
		}
	}

	// On slot clicked
	public void OnSlotClicked() {
		if (!isActionMenuOpen && selectedSlot < items.Count) {
			OpenActionMenu(true);
		}
	}

	void OpenActionMenu(bool open) {
		isActionMenuOpen = open;
		SelectActionMenuEntry(0);
		actionMenu.SetActive(open);
	}

	// On action menu cancel clicked
	public void OnCloseActionMenu() {
		OpenActionMenu(false);
	}

	// On action menu entry hovered
	public void OnActionMenuEntryHover(int index) {
		SelectActionMenuEntry(index);
	}

	void SelectActionMenuEntry(int index) {
		actionMenuSelectionTransform.SetParent(actionMenu.transform.GetChild(index), false);
	}

	// On action menu drop clicked
	public void OnDropActionMenu() {
		if (selectedSlot >= items.Count) {
			throw new UnityException("Assert: Try to drop a non selected item, should not happend");
		}

		inventory.DropItem(items[selectedSlot]);
		OpenActionMenu(false);
		ShowItems();
		RefreshSlotSelection();
	}

}
