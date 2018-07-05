using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : StorageContainer {

	public GameObject playerHand;
	public int hotbarKeyPressed;

	int selected = 0;
	PlayerHand playerHandScript;
	KeyCode[] keycodes;

	protected override void Start() {
		keycodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
		playerHandScript = playerHand.GetComponent<PlayerHand>();

		base.Start();
	}

	public override void SetupSlot(int number) {
		base.SetupSlot(number);

		itemSlots[number].slot.GetComponent<Button>().interactable = false;
	}

	void Update() {
		if (playerScript.playerState == Player.PlayerState.Inventory && itemSlots[0].slotItemImage.raycastTarget == false) {
			for (int i = 0; i < itemSlots.Length; i++) {
				itemSlots[i].slotItemImage.raycastTarget = true;
			}
		}
		else if(playerScript.playerState != Player.PlayerState.Inventory && itemSlots[0].slotItemImage.raycastTarget == true) {
			for (int i = 0; i < itemSlots.Length; i++) {
				itemSlots[i].slotItemImage.raycastTarget = false;
			}
		}

		if ((int)Input.GetAxis("Mouse ScrollWheel") != 0) {
			selected += (int)Input.GetAxis("Mouse ScrollWheel");
			UpdateHotbar();
		}

		if (Input.anyKey) {
			hotbarKeyPressed = -1;
			for (int i = 0; i < keycodes.Length; i++) {
				if (Input.GetKeyDown(keycodes[i])) {
					hotbarKeyPressed = i;
					selected = i;
					UpdateHotbar();
				}
			}
		}
	}

	void UpdateHotbar() {
		if (selected > slots - 1) selected = 0;
		if (selected < 0) selected = slots - 1;
		playerHandScript.item = itemSlots[selected].item;

		for (int i = 0; i < slots; i++) {
			if (i == selected) {
				itemSlots[i].slot.GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1.1f);
			}
			else {
				itemSlots[i].slot.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
			}
		}
	}

	public override void UpdateSlots() {
		base.UpdateSlots();

		UpdateHotbar();
	}
}
