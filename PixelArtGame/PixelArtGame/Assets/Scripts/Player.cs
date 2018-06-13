using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public enum PlayerState : short {
		Walking,
		Dashing,
		Attacking
	}
	public PlayerState playerState = PlayerState.Walking;

	public float speed;
	public Vector2 facing;

	Vector2 movement;
	Rigidbody2D rb;
	Attack attackScript;
	private float attackCharge;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		attackScript = GetComponentInChildren<Attack>();
	}

	void Update() {
		facing = new Vector2(GameManager.instance.mousePos.x - transform.position.x, GameManager.instance.mousePos.y - transform.position.y).normalized;
		movement = Vector2.zero;

		switch (playerState) {
			case PlayerState.Walking:
				Movement();
				break;
		}

		Attack();
	}

	void FixedUpdate() {
		rb.AddForce(movement);
	}

	private void Movement() {
		movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (movement.magnitude > 1) {
			movement = movement.normalized;
		}
		movement *= speed;
	}

	private void Attack() {
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			attackScript.SimpleAttack();
		}
		if (Input.GetKey(KeyCode.LeftShift)) {
			attackCharge += Time.deltaTime;
			if (attackCharge >= 2) {
				if (Input.GetKeyDown(KeyCode.Mouse0)) {
					attackScript.DashAttack();
				}
			}
		}
		else {
			attackCharge = 0;
		}
	}
}