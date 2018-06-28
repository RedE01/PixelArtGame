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
			spriteRenderer.sprite = item.sprite;
		}
		else {
			spriteRenderer.sprite = null;
		}
	}
}
