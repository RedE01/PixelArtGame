using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

	public ChestTier chestTier;
	[HideInInspector]
	public Item[] chestItems;
	[HideInInspector]
	public int[] itemCount;

	[HideInInspector]
	public bool interactable;

	Player playerScript;
	ChestUI chestUI;

	void Start() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player>();
		chestUI = player.GetComponent<ChestUI>();

		int items = chestUI.slots;
		chestItems = new Item[items];
		itemCount = new int[items];

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
				int count = Random.Range(1, slotItem.stackSize);
				itemCount[i] = count;
			}
			else {
				itemCount[i] = 0;
			}
		}
	}
}
