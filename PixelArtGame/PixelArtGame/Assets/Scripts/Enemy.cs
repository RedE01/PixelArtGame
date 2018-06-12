using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health;

	GameObject target;
	Rigidbody2D rb;

	void Start() {
		target = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
	}

	public bool Damage(Vector2 attackPosition, int damageAmmount) {
		health -= damageAmmount;
		Vector2 dmgVector = new Vector2(transform.position.x - attackPosition.x, transform.position.y - attackPosition.y).normalized;
		rb.AddForce(dmgVector * 100);

		if(health <= 0) {
			Destroy(gameObject, .5f);
			return true;
		}
		return false;
	}

}
