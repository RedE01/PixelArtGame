using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;

	Vector2 movement;
	Rigidbody2D rb;
	Attack attackScript;
	private float attackCharge;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		attackScript = GetComponentInChildren<Attack>();
	}

	void Update() {
		Movement();

		//Attack
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			attackScript.SwordParticles();
			attackScript.Invoke("HitEnemy", attackScript.hitTime);
		}
		if (Input.GetKey(KeyCode.LeftShift)) {
			attackCharge += Time.deltaTime;
			if(attackCharge >= 2) {
				if(Input.GetKeyDown(KeyCode.Mouse0)) {
					//Dash attack
				}
			}
		}
		else {
			attackCharge = 0;
		}

	}

	void FixedUpdate() {
		rb.MovePosition(movement);
	}

	void Movement() {
		movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (movement.magnitude > 1) {
			movement = movement.normalized;
		}
		movement *= speed / 100;
		movement.x += transform.position.x;
		movement.y += transform.position.y;
	}
}