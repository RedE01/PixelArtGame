using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour {

	public GameObject dropPrefab;
	public Item paperScroll;
	public Transform playerTransform;

	Vector2 currentMousePos;
	Vector2 previousMousePos;
	RectTransform rectTransform;
	Rect parentRect;

	void Start() {
		currentMousePos = Input.mousePosition;
		previousMousePos = Input.mousePosition;
		rectTransform = GetComponent<RectTransform>();
		parentRect = transform.parent.GetComponent<RectTransform>().rect;
	}

	public void SetMousePos() {
		currentMousePos = Input.mousePosition;
		previousMousePos = Input.mousePosition;
	}

	public void FollowCursor() {
		currentMousePos = Input.mousePosition;
		float x = currentMousePos.x - previousMousePos.x;
		float y = currentMousePos.y - previousMousePos.y;
		previousMousePos = currentMousePos;

		if(Mathf.Abs(rectTransform.anchoredPosition.x + x) > (parentRect.width - rectTransform.rect.width) * 0.5f) {
			return;
		}
		if(Mathf.Abs(rectTransform.anchoredPosition.y + x) > (parentRect.height - rectTransform.rect.height) * 0.5f) {
			return;
		}

		transform.Translate(x, y, 0);
	}

	public void GiveQuest() {
		GameObject p = GameObject.Instantiate(dropPrefab, playerTransform.position, Quaternion.identity);
		p.GetComponent<Pickup>().item = paperScroll;
		Destroy();
	}

	public void Destroy() {
		GameObject.Destroy(gameObject);
	}
}
