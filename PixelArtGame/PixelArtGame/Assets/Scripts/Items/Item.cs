using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject {

	public enum ItemType {
		Item,
		Axe,
	}
	public ItemType itemType;

	public new string name;
	public Sprite inventorySprite;
	public Sprite equippedSprite;

	[Range(0.4f, 2)]
	public float pickupRadius = 0.4f;
}
