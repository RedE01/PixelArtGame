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

	Inventory inventoryScript;
	Hotbar hotbarScript;
	RectTransform rTransform;

	void Start() {
		inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		hotbarScript = inventoryScript.GetComponent<Hotbar>();
		rTransform = GetComponent<RectTransform>();
	}

	public void FollowCursor() {
		transform.position = Input.mousePosition;
		transform.parent.SetAsLastSibling();
		transform.parent.parent.SetAsLastSibling();
	}

	public void ItemRelease() {
		RaycastHit2D hit = Physics2D.Raycast((Input.mousePosition), Vector2.zero);

		if (hit.collider == null) {
			DropItem();
		}
		else if (hit.collider.name == "InventoryBackground" || hit.collider.name == "CharacterSlotBackground") {
			PlaceItemInSlot(inventoryScript);
		}
		else if(hit.collider.name == "HotbarBackground") {
			PlaceItemInSlot(hotbarScript);
		}
		containerScript.UpdateSlots();
		rTransform.anchoredPosition = Vector2.zero;
	}

	void PlaceItemInSlot(StorageContainer contScript) {
		Vector3 nearest = Vector2.zero;
		int nearestSlotNumb = 0;
		Vector3 offset = containerScript.itemSlots[slotNumber].slotRectTransform.sizeDelta * 0.5f;
		offset.y *= -1;
		for (int i = 0; i < contScript.itemSlots.Length; i++) {
			if (Vector2.Distance(transform.position, contScript.itemSlots[i].slotRectTransform.position + offset) < Vector2.Distance(transform.position, nearest + offset)) {
				nearest = contScript.itemSlots[i].slotRectTransform.position;
				nearestSlotNumb = i;
			}
		}
		
		//Placing items on empty or same item
		if (contScript.itemSlots[nearestSlotNumb].item == null || (contScript.itemSlots[nearestSlotNumb].item == containerScript.itemSlots[slotNumber].item && contScript.itemSlots[nearestSlotNumb].itemCount < contScript.maxItems)) {
			if (nearestSlotNumb != slotNumber || contScript != containerScript) {
				contScript.itemSlots[nearestSlotNumb].item = containerScript.itemSlots[slotNumber].item;

				contScript.itemSlots[nearestSlotNumb].itemCount += containerScript.itemSlots[slotNumber].itemCount;
				containerScript.itemSlots[slotNumber].itemCount = contScript.itemSlots[nearestSlotNumb].itemCount - inventoryScript.maxItems;

				if (contScript.itemSlots[nearestSlotNumb].itemCount >= contScript.maxItems) contScript.itemSlots[nearestSlotNumb].itemCount = contScript.maxItems;
				if (containerScript.itemSlots[slotNumber].itemCount <= 0) containerScript.itemSlots[slotNumber].item = null;

				contScript.UpdateSlots();
			}
		}
		//Switching items with different item
		else if(contScript.itemSlots[nearestSlotNumb].item != containerScript.itemSlots[slotNumber].item && contScript.itemSlots[nearestSlotNumb].item != null) {
			Item tempItem = contScript.itemSlots[nearestSlotNumb].item;
			int tempItemCount = contScript.itemSlots[nearestSlotNumb].itemCount;

			contScript.itemSlots[nearestSlotNumb].item = containerScript.itemSlots[slotNumber].item;
			containerScript.itemSlots[slotNumber].item = tempItem;

			contScript.itemSlots[nearestSlotNumb].itemCount = containerScript.itemSlots[slotNumber].itemCount;
			containerScript.itemSlots[slotNumber].itemCount = tempItemCount;

			contScript.UpdateSlots();
		}
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
