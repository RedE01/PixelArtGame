using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;

	Vector2 movement;
	Rigidbody2D rb;
	GameObject attackParticle;
	ParticleSystem attackParticleSystem;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		attackParticle = GameObject.Find("AttackParticle");
		attackParticleSystem = attackParticle.GetComponent<ParticleSystem>();
	}

	void Update() {
		Movement();
		Attack();
		
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

	void Attack() {
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 partDir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;

			Vector2 partPos = new Vector2(partDir.x + transform.position.x, partDir.y + transform.position.y);
			attackParticle.transform.position = partPos;

			var partMain = attackParticleSystem.main;
			partMain.startRotation = Mathf.Atan2(partDir.x, partDir.y) - 2.25f;

			attackParticleSystem.Play();
		}
	}
}