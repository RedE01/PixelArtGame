using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : StorageContainer {

	public int armorSlots;

	public GameObject helmetSlot;
	public GameObject chestplateSlot;
	public GameObject leggingsSlot;
	public GameObject bootsSlot;

	new void Start() {
		Initialize();
		itemSlots = new ItemSlot[slots + armorSlots];

		itemSlots[slots + 0].slot = helmetSlot;
		itemSlots[slots + 1].slot = chestplateSlot;
		itemSlots[slots + 2].slot = leggingsSlot;
		itemSlots[slots + 3].slot = bootsSlot;

		for (int i = slots; i < slots + armorSlots; i++) {
			SetupSlot(i);
		}

		CreateSlotGrid("slot");

		UpdateSlots(); 
	}

	void Update() {
		if(Input.GetButtonDown("Inventory") && playerScript.playerState != Player.PlayerState.Inventory) {
			parentObject.SetActive(true);
			playerScript.playerState = Player.PlayerState.Inventory;
			UpdateSlots();
		}
		else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Inventory")) {
			parentObject.SetActive(false);
			playerScript.playerState = Player.PlayerState.Default;
			UpdateSlots();
		}

	}

	public override void UpdateSlots() {
		base.UpdateSlots();
		for (int i = 0; i < itemSlots.Length; i++) {
			if (i > slots - 1) {
				ArmorItems(i);
			}
		}
	}

	void ArmorItems(int number) {
		if (itemSlots[number].item != null) {
			switch(itemSlots[number].slot.name) {
				case "HelmetSlot":
					break;
				case "ChestplateSlot":
					break;
				case "LeggingsSlot":
					break;
				case "BootsSlot":
					break;
			}
		}
		else {
			
		}
	}
}
