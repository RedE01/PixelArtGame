using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour {

	SpriteRenderer objectSpriteRenderer;

	protected virtual void Start () {
		objectSpriteRenderer = SetRenderer();
	}
	
	void LateUpdate () {
		objectSpriteRenderer.sortingOrder = -(int)(transform.position.y * 100); // * x adds x times the amount of layer precision
	}

	protected abstract SpriteRenderer SetRenderer();
}
