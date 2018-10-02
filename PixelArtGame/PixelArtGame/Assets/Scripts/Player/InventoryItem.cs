using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {

	public Pickup pickup;

	[HideInInspector]
	public int slotNumber;
	[HideInInspector]
	public StorageContainer containerScript;

	RectTransform UITransform;
	Inventory inventoryScript;
	Hotbar hotbarScript;
	ChestUI chestUIScript;
	RectTransform rTransform;
	bool mouseOver;

	void Start() {
		inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		hotbarScript = inventoryScript.GetComponent<Hotbar>();
		chestUIScript = inventoryScript.GetComponent<ChestUI>();
		rTransform = GetComponent<RectTransform>();
		UITransform = GameObject.Find("UI").GetComponent<RectTransform>();
	}

	void OnDisable() {
		if(rTransform != null)
			rTransform.anchoredPosition = Vector2.zero;
	}

	void Update() {
		if(mouseOver) {
			MoveToHotbar();
		}
	}

	public void FollowCursor() {
		transform.position = Input.mousePosition;
		transform.parent.SetAsLastSibling();
		transform.parent.parent.SetAsLastSibling();
		containerScript.description.gameObject.SetActive(false);
	}

	public void MouseEnter() {
		mouseOver = true;
		containerScript.description.gameObject.SetActive(true);
		containerScript.description.GetChild(0).GetComponent<Text>().text = containerScript.itemSlots[slotNumber].item.description;
		containerScript.description.SetAsLastSibling();
		containerScript.description.position = transform.position;
	}

	public void MouseLeave() {
		mouseOver = false;
		containerScript.description.gameObject.SetActive(false);
	}

	public void ItemRelease() {
		RaycastHit2D hit = Physics2D.Raycast((Input.mousePosition), Vector2.zero);

		if (hit.collider == null) {
			DropItem();
		}
		else if (hit.collider.name == "InventoryBackground" || hit.collider.name == "CharacterSlotBackground") {
			PlaceItemInSlot(inventoryScript, GetNearestSlot(inventoryScript));
		}
		else if(hit.collider.name == "HotbarBackground") {
			PlaceItemInSlot(hotbarScript, GetNearestSlot(hotbarScript));
		}
		else if (hit.collider.name == "ChestContainer") {
			PlaceItemInSlot(chestUIScript, GetNearestSlot(chestUIScript));
		}
		containerScript.UpdateSlots();
		rTransform.anchoredPosition = Vector2.zero;
	}

	void MoveToHotbar() {
		if (hotbarScript.hotbarKeyPressed != -1) {
			PlaceItemInSlot(hotbarScript, hotbarScript.hotbarKeyPressed);
			containerScript.UpdateSlots();
			rTransform.anchoredPosition = Vector2.zero;
		}
	}

	int GetNearestSlot(StorageContainer contScript) {
		Vector3 nearest = Vector2.zero;
		int nearestSlotNumb = 0;
		float UIscale = UITransform.localScale.x;
		Vector3 offset = containerScript.itemSlots[slotNumber].slotRectTransform.sizeDelta * 0.5f * UIscale;
		offset.y *= -1f;
		for (int i = 0; i < contScript.itemSlots.Length; i++) {
			if (Vector2.Distance(transform.position, contScript.itemSlots[i].slotRectTransform.position + offset) < Vector2.Distance(transform.position, nearest + offset)) {
				nearest = contScript.itemSlots[i].slotRectTransform.position;
				nearestSlotNumb = i;
			}
		}
		return nearestSlotNumb;
	}

	void PlaceItemInSlot(StorageContainer contScript, int nearestSlotNumb) {
		int maxItemStack = containerScript.itemSlots[slotNumber].item.stackSize;
		Item currentItem = containerScript.itemSlots[slotNumber].item;
		Item targetItem = contScript.itemSlots[nearestSlotNumb].item;
		int currentItemCount = containerScript.itemSlots[slotNumber].itemCount;
		int targetItemCount = contScript.itemSlots[nearestSlotNumb].itemCount;

		//Placing items on empty or same item
		if (targetItem == null || (targetItem == currentItem && targetItemCount < maxItemStack)) {
			if (nearestSlotNumb != slotNumber || contScript != containerScript) {
				targetItem = currentItem;

				targetItemCount += currentItemCount;
				currentItemCount = targetItemCount - maxItemStack;

				if (targetItemCount >= maxItemStack) targetItemCount = maxItemStack;
				if (currentItemCount <= 0) currentItem = null;
			}
		}
		//Switching items with different item
		else if(targetItem != currentItem && targetItem != null) {
			Item tempItem = targetItem;
			int tempItemCount = targetItemCount;

			targetItem = currentItem;
			currentItem = tempItem;

			targetItemCount = currentItemCount;
			currentItemCount = tempItemCount;
		}

		containerScript.itemSlots[slotNumber].item = currentItem;
		contScript.itemSlots[nearestSlotNumb].item = targetItem;
		containerScript.itemSlots[slotNumber].itemCount = currentItemCount;
		contScript.itemSlots[nearestSlotNumb].itemCount = targetItemCount;

		contScript.UpdateSlots();
	}

	void DropItem() {
		for (int i = 0; i < inventoryScript.itemSlots[slotNumber].itemCount; i++) {
			Vector2 dropPos = inventoryScript.transform.position;
			dropPos.x += Random.Range(-.5f, .5f);
			dropPos.y -= Random.Range(.5f, 1f);
			Pickup p = Instantiate(pickup, dropPos, Quaternion.identity);
			p.item = inventoryScript.itemSlots[slotNumber].item;
		}
		inventoryScript.itemSlots[slotNumber].item = null;
	}
}
