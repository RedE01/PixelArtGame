using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour {

	[HideInInspector]
	public Item item;

	public SpriteRenderer playerSpriteRenderer;

	SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateHand();
	}

	void Update() {
		UpdateHand();
	}

	void FixedUpdate() {
		spriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 10;
	}

	void UpdateHand() {
		if (item != null) {
			spriteRenderer.sprite = item.equippedSprite;
		}
		else {
			spriteRenderer.sprite = null;
		}
	}

	public Item.ItemType GetItemType(Item item) {
		if (item != null) {
			return item.itemType;
		}
		else {
			return Item.ItemType.Item;
		}
	}
}
