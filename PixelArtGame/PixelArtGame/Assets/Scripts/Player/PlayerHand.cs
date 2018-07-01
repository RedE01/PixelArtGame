using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour {

	public Item item;

	Player playerScript;
	SpriteRenderer playerSpriteRenderer;
	SpriteRenderer spriteRenderer;

	void Start() {
		playerScript = transform.parent.GetComponent<Player>();
		playerSpriteRenderer = playerScript.GetComponent<SpriteRenderer>();
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

			switch(item.itemType) {
				case Item.ItemType.Item:
					playerScript.playerState = Player.PlayerState.Walking;
					break;
				case Item.ItemType.Axe:
					playerScript.playerState = Player.PlayerState.Destroying;
					break;
				case Item.ItemType.BuildingMaterial:
					playerScript.playerState = Player.PlayerState.Building;
					break;
				default:
					playerScript.playerState = Player.PlayerState.Walking;
					break;
			}
		}
		else {
			spriteRenderer.sprite = null;
		}
	}
}
