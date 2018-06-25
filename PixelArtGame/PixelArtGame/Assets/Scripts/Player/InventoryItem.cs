using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {

	public Pickup pickup;

	[HideInInspector]
	public int slotNumber;

	Inventory inventoryScript;
	RectTransform rTransform;

	void Start() {
		inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		rTransform = GetComponent<RectTransform>();
	}

	public void FollowCursor() {
		transform.position = Input.mousePosition;
		transform.parent.SetAsLastSibling();
	}

	public void ItemRelease() {
		RaycastHit2D hit = Physics2D.Raycast((Input.mousePosition), Vector2.zero);

		if (hit.collider != null) {
			PlaceItemInSlot();
		}
		else {
			DropItem();
		}
		inventoryScript.UpdateInventory();
		rTransform.anchoredPosition = Vector2.zero;
	}

	void PlaceItemInSlot() {
		Vector3 nearest = Vector2.zero;
		int nearestSlotNumb = 0;
		Vector3 offset = inventoryScript.itemSlots[slotNumber].slotRectTransform.sizeDelta * 0.5f;
		offset.y *= -1;
		for (int i = 0; i < inventoryScript.itemSlots.Length; i++) {
			if (Vector2.Distance(transform.position, inventoryScript.itemSlots[i].slotRectTransform.position + offset) < Vector2.Distance(transform.position, nearest + offset)) {
				nearest = inventoryScript.itemSlots[i].slotRectTransform.position;
				nearestSlotNumb = i;
			}
		}
		if (inventoryScript.itemSlots[nearestSlotNumb].item == null || (inventoryScript.itemSlots[nearestSlotNumb].item == inventoryScript.itemSlots[slotNumber].item && inventoryScript.itemSlots[nearestSlotNumb].itemCount < inventoryScript.maxItems)) {
			if (nearestSlotNumb != slotNumber) {
				inventoryScript.itemSlots[nearestSlotNumb].item = inventoryScript.itemSlots[slotNumber].item;

				inventoryScript.itemSlots[nearestSlotNumb].itemCount += inventoryScript.itemSlots[slotNumber].itemCount;
				inventoryScript.itemSlots[slotNumber].itemCount = inventoryScript.itemSlots[nearestSlotNumb].itemCount - inventoryScript.maxItems;

				if (inventoryScript.itemSlots[nearestSlotNumb].itemCount >= inventoryScript.maxItems) inventoryScript.itemSlots[nearestSlotNumb].itemCount = inventoryScript.maxItems;
				if (inventoryScript.itemSlots[slotNumber].itemCount <= 0) inventoryScript.itemSlots[slotNumber].item = null;
			}
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
