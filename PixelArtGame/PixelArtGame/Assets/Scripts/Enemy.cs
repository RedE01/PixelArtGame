using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health, damageKnockback;
	public float speed;

	GameObject target;
	Rigidbody2D rb;
	Vector2 movement;

	void Start() {
		target = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		movement = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;
	}

	void FixedUpdate() {
		rb.AddForce(movement * speed);
	}

	public bool Damage(Vector2 attackPosition, int damageAmmount) {
		health -= damageAmmount;
		Vector2 dmgVector = new Vector2(transform.position.x - attackPosition.x, transform.position.y - attackPosition.y).normalized;
		rb.AddForce(dmgVector * damageKnockback);

		if(health <= 0) {
			Destroy(gameObject, .5f);
			return true;
		}
		return false;
	}

}