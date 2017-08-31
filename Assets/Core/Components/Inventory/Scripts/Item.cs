using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item")]
public class Item : ScriptableObject {

	public enum Type {
		Gun,
		Clothe,
		Ingredient,
		Blueprint,
		Special
	}

	public Type type;
	public string itemName;
	[TextArea(3,10)] public string itemDescription;
	public Sprite sprite;
	public GameObject prefab;
	public bool isStackable;

	public override string ToString() {
		return itemName;
	}

}
