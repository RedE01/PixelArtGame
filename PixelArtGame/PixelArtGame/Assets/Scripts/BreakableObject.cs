using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour {

	public Item drop;
	public GameObject PickupPrefab;

	public void Destroy(int dropsMin, int dropsMax) {
		int dropsCount = Mathf.RoundToInt(Random.Range(dropsMin, dropsMax));

		for (int i = 0; i < dropsCount; i++) {
			Vector2 pos = transform.position;
			pos.x += Random.Range(-1f, 1f);
			pos.y += Random.Range(-1f, 1f);
			Pickup p = Instantiate(PickupPrefab, pos, Quaternion.identity).GetComponent<Pickup>();
			p.item = drop;
		}
		Destroy(gameObject);
	}

}
