using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

	public ChestTier chestTier;
	public Item[] chestItems;
	public int[] itemCount;

	[HideInInspector]
	public bool interactable;

	Player playerScript;
	ChestUI chestUI;

	void Start() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player>();
		chestUI = player.GetComponent<ChestUI>();

		chestItems = new Item[chestUI.itemSlots.Length];
		itemCount = new int[chestUI.itemSlots.Length];

		GenerateChest();
	}

	void Update() {
		if (interactable) {
			if (Input.GetButtonDown("Interact") && playerScript.playerState != Player.PlayerState.Inventory) {
				chestUI.OpenChest(this);
			}
		}
	}

	void GenerateChest() {
		for(int i = 0; i < chestItems.Length; i++) {
			float percent = Random.Range(0, 100);
			Item slotItem = null;
			float itemNumberPercent = 0;
			for (int j = 0; j < chestTier.item.Length; j++) {
				itemNumberPercent += chestTier.percentPerSlot[j];
				if (itemNumberPercent >= percent) {
					slotItem = chestTier.item[j];
					break;
				}
			}

			if (slotItem != null) {
				chestItems[i] = slotItem;
				itemCount[i] = 1;
			}
			else {
				itemCount[i] = 0;
			}
		}
	}
}
