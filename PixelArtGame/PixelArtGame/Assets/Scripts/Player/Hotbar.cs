using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : StorageContainer {

	public GameObject playerHand;

	int selected = 0;
	PlayerHand playerHandScript;
	KeyCode[] keycodes;

	protected override void Start() {
		keycodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
		playerHandScript = playerHand.GetComponent<PlayerHand>();

		base.Start();
	}

	void Update() {
		if ((int)Input.GetAxis("Mouse ScrollWheel") != 0) {
			selected += (int)Input.GetAxis("Mouse ScrollWheel");
			UpdateHotbar();
		}

		if (Input.anyKey) {
			for (int i = 0; i < keycodes.Length; i++) {
				if (Input.GetKeyDown(keycodes[i])) {
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
