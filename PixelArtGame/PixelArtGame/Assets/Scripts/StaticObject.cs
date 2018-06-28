using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : MonoBehaviour {

	void Start () {
		GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y * GameManager.instance.sortingOrderPrecision);
	}
}
