using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Warfest;

public class Inventory : MonoBehaviour {

	public List<Item> items;

	Transform dropItemParent;

	void Start() {
		dropItemParent = GameObject.Find("/ItemsHolder").transform;
	}

	public List<Item> GetItems() {
		return items;
	}

	public List<Item> GetItems(Item.Type type) {
		return items.Where(e => e.type == type).ToList();
	}

	public void AddItem(Item item) {
		items.Add(item);
	}

	public void DropItem(Item item) {
		Instantiate(
			item.prefab,
			transform.position - transform.forward, // Drop it one meter behind the player.
			item.prefab.transform.rotation,  // Keep the rotation of the prfab.
			dropItemParent
		);

		items.Remove(item);
	}

	public void EquipItem(Item item) {
		Debug.Log("[Inventory] EquipItem: " + item);

		ItemBlock itemBlock = item.prefab.GetComponent<ItemBlock>();

		if (itemBlock != null) {
			GetComponent<EditTerrain>().SetColor(itemBlock.color);
		}
	}

}
