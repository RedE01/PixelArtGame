using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour {

	public SpriteRenderer objectSpriteRenderer;
	public float yFeetPos;
	
	void LateUpdate () {
		objectSpriteRenderer.sortingOrder = -(int)((transform.position.y + yFeetPos) * 100); // * x adds x times the amount of layer precision
	}
}
