﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public GameObject inventory;
	public GameObject slot;
	public int slots;
	public int maxItems;
	public int slotSize;
	public int slotOffset;
	public int topOffset;

	public ItemSlot[] itemSlots;

	Player playerScript;

	void Start() {
		playerScript = GetComponent<Player>();
		itemSlots = new ItemSlot[slots];

		Vector2 pos = new Vector2(slotOffset, -topOffset);
		for (int i = 0; i < itemSlots.Length; i++) {
			itemSlots[i].slot = Instantiate(slot, Vector2.zero, Quaternion.identity, inventory.transform);
			itemSlots[i].slot.name = "Slot";
			itemSlots[i].slotRectTransform = itemSlots[i].slot.GetComponent<RectTransform>();
			itemSlots[i].slotItemImage = itemSlots[i].slot.transform.Find("Item").GetComponent<Image>();
			itemSlots[i].slotCounterText = itemSlots[i].slotItemImage.transform.Find("ItemCounter").GetComponent<Text>();
			itemSlots[i].invItem = itemSlots[i].slotItemImage.GetComponent<InventoryItem>();
			itemSlots[i].invItem.slotNumber = i;

			itemSlots[i].slotRectTransform.anchoredPosition = pos;

			pos.x += slotOffset + slotSize;
			if (pos.x + slotSize > inventory.GetComponent<RectTransform>().sizeDelta.x) {
				pos.x = slotOffset;
				pos.y -= slotOffset + slotSize;
			}
		}
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.E)) {
			inventory.SetActive(!inventory.activeSelf);
			playerScript.playerState = inventory.activeSelf ? Player.PlayerState.Menu : Player.PlayerState.Walking;
			UpdateInventory();
		}
	}
	public void UpdateInventory() {
		for (int i = 0; i < itemSlots.Length; i++) {
			if (itemSlots[i].item != null) {
				itemSlots[i].slotItemImage.gameObject.SetActive(true);

				itemSlots[i].slotItemImage.sprite = itemSlots[i].item.sprite;
				itemSlots[i].slotCounterText.text = itemSlots[i].itemCount.ToString();
			}
			else {
				itemSlots[i].slotItemImage.gameObject.SetActive(false);
				itemSlots[i].itemCount = 0;
			}
		}
	}
}

public struct ItemSlot {
	public GameObject slot;
	public RectTransform slotRectTransform;
	public Image slotItemImage;
	public Text slotCounterText;
	public Item item;
	public InventoryItem invItem;
	public int itemCount;
}
