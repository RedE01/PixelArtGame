using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : StorageContainer {

	public GameObject[] characterSlots;
	private Chest currentChest;

	new void Start() {
		Initialize();

		CreateSlotGrid("chestSlot");

		UpdateSlots();
	}

	void Update() {
		if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Inventory")) {
			foreach (GameObject o in characterSlots) {
				o.SetActive(true);
			}
			foreach (ItemSlot i in itemSlots) {
				i.slot.SetActive(false);
			}
			parentObject.SetActive(false);

			if (currentChest != null) {
				for (int i = 0; i < itemSlots.Length; i++) {
					currentChest.chestItems[i] = itemSlots[i].item;
					currentChest.itemCount[i] = itemSlots[i].itemCount;
				}
				currentChest = null;
			}
		}
	}

	public void OpenChest(Chest chest) {
		currentChest = chest;
		inventoryObject.SetActive(true);
		playerScript.playerState = Player.PlayerState.Inventory;

		foreach (ItemSlot i in itemSlots) {
			i.slot.SetActive(true);
		}
		foreach (GameObject o in characterSlots) {
			o.SetActive(false);
		}
		parentObject.SetActive(true);
		for (int i = 0; i < itemSlots.Length; i++) {
			itemSlots[i].item = currentChest.chestItems[i];
			itemSlots[i].itemCount = currentChest.itemCount[i];
		}

		UpdateSlots();
	}
				
}
