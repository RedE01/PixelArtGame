using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour {

	public SpriteRenderer objectSpriteRenderer;
	public float yFeetPos;
	
	void LateUpdate () {
		objectSpriteRenderer.sortingOrder = (int)((transform.position.y + yFeetPos) * GameManager.instance.sortingOrderPrecision);
	}
}
