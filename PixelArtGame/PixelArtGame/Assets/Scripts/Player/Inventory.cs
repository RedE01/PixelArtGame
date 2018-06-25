using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : StorageContainer {

	public int armorSlots;

	[Header("ArmorSlots")]
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

		CreateSlotGrid();

		UpdateInventory(); 
	}

	void Update() {
		if(Input.GetButtonDown("Inventory")) {
			parentObject.SetActive(!parentObject.activeSelf);
			playerScript.playerState = parentObject.activeSelf ? Player.PlayerState.Menu : Player.PlayerState.Walking;
			UpdateInventory();
		}
		if (Input.GetButtonDown("Cancel")) {
			parentObject.SetActive(false);
			playerScript.playerState = Player.PlayerState.Walking;
			UpdateInventory();
		}

	}

	public new void UpdateInventory() {
		base.UpdateInventory();
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
