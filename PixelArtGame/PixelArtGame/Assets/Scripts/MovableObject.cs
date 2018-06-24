using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour {

	public SpriteRenderer objectSpriteRenderer;
	
	void LateUpdate () {
		objectSpriteRenderer.sortingOrder = (int)((transform.position.y) * GameManager.instance.sortingOrderPrecision);
	}
}
