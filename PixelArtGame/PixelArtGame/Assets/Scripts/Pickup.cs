using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MovableObject {

	public Item item;
	public float speed, height;
	public Sprite shadow;

	GameObject pickupObj, shadowObj;
	SpriteRenderer shadowRenderer, pickupRenderer;

	void Start() {
		pickupObj = transform.Find("PickupSprite").gameObject;
		pickupRenderer = pickupObj.GetComponent<SpriteRenderer>();
		pickupRenderer.sprite = item.sprite;

		shadowObj = transform.Find("ShadowSprite").gameObject;
		shadowRenderer = shadowObj.GetComponent<SpriteRenderer>();
		shadowRenderer.sprite = shadow;

		GetComponent<CircleCollider2D>().radius = item.pickupRadius;
	}

	void Update() {
		float sinCurve = Mathf.Sin(Time.time * speed);
		float shadowCurve = (-sinCurve * 0.3f + 2) * item.pickupRadius;
		shadowObj.transform.localScale = new Vector2(shadowCurve, shadowCurve);
		pickupObj.transform.localPosition = new Vector2(0, sinCurve * height + height * 2.5f);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.CompareTag("Player")) {
			GameManager.instance.coins++;

			Destroy(gameObject);
		}
	}
}
