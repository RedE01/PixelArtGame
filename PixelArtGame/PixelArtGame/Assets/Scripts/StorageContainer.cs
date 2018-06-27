using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StorageContainer : MonoBehaviour {

	public GameObject parentObject;
	public GameObject slot;
	public int slots;
	public int maxItems;
	public int slotOffset;
	public int topOffset;

	int slotSize;

	public ItemSlot[] itemSlots;

	protected Player playerScript;

	protected virtual void Start() {
		Initialize();

		CreateSlotGrid();
		UpdateSlots();
	}

	protected void Initialize() {
		itemSlots = new ItemSlot[slots];
		playerScript = GetComponent<Player>();
		slotSize = (int)slot.GetComponent<RectTransform>().sizeDelta.x;
	}

	protected void CreateSlotGrid() {
		Vector2 pos = new Vector2(slotOffset, -topOffset);
		for (int i = 0; i < slots; i++) {
			itemSlots[i].slot = Instantiate(slot, Vector2.zero, Quaternion.identity, parentObject.transform);
			itemSlots[i].slot.name = "Slot";

			SetupSlot(i);

			itemSlots[i].slotRectTransform.anchoredPosition = pos;

			pos.x += slotOffset + slotSize;
			if (pos.x + slotSize > parentObject.GetComponent<RectTransform>().sizeDelta.x) {
				pos.x = slotOffset;
				pos.y -= slotOffset + slotSize;
			}
		}
	}

	protected void SetupSlot(int number) {
		itemSlots[number].slotRectTransform = itemSlots[number].slot.GetComponent<RectTransform>();
		itemSlots[number].slotItemImage = itemSlots[number].slot.transform.Find("Item").GetComponent<Image>();
		itemSlots[number].slotCounterText = itemSlots[number].slotItemImage.transform.Find("ItemCounter").GetComponent<Text>();
		itemSlots[number].invItem = itemSlots[number].slotItemImage.GetComponent<InventoryItem>();
		itemSlots[number].invItem.slotNumber = number;
		itemSlots[number].invItem.containerScript = this;
	}

	public virtual void UpdateSlots() {
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
