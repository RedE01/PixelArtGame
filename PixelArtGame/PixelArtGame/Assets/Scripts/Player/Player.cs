using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovableObject {

	public enum PlayerState : short {
		Walking,
		Building,
		Destroying,
		Dashing,
		Attacking,
		Menu
	}
	public PlayerState playerState = PlayerState.Walking;

	public float speed, dashChargeupTime;

	[HideInInspector]
	public Vector2 facing;

	HealthUI healthbar;
	Vector2 movement;
	Rigidbody2D rb;
	Attack attackScript;
	EditWorld buildScript;
	CameraScript cameraScript;
	float dashChargeup;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		attackScript = GetComponentInChildren<Attack>();
		cameraScript = Camera.main.GetComponent<CameraScript>();
		buildScript = GetComponentInChildren<EditWorld>();
		healthbar = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<HealthUI>();
	}

	void Update() {
		facing = new Vector2(GameManager.instance.mousePos.x - transform.position.x, GameManager.instance.mousePos.y - transform.position.y).normalized;
		movement = Vector2.zero;

		float facingRad = Mathf.Atan2(facing.x, facing.y);
		attackScript.transform.rotation = Quaternion.AngleAxis(-facingRad * Mathf.Rad2Deg, new Vector3(0, 0, 1));

		switch (playerState) {
			case PlayerState.Walking:
				Movement();
				Attack();
				break;
			case PlayerState.Building:
				Movement();
				Build();
				break;
			case PlayerState.Destroying:
				Movement();
				DestroyObject();
				break;
			case PlayerState.Menu:
				Movement();
				break;
		}
	}

	void FixedUpdate() {
		rb.AddForce(movement);
	}

	public void Damage(float damageAmmount, Vector2 damagePos, float damageKnockback) {
		GameManager.instance.health -= damageAmmount;
		rb.AddForce(((Vector2)transform.position - damagePos) * damageKnockback, ForceMode2D.Impulse);
		cameraScript.StartShake(1, 10, 0.08f);
		healthbar.UpdateHealthBar(GameManager.instance.health);

		if(GameManager.instance.health <= 0) {
			Debug.Log("GAME OVER");
		}
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
			if (!CheckUIClick()) {
				attackScript.SimpleAttack();
			}
		}
		if (Input.GetKey(KeyCode.LeftShift)) {
			dashChargeup += Time.deltaTime;

			if (dashChargeup >= dashChargeupTime) {
				if (Input.GetKeyDown(KeyCode.Mouse0)) {
					if (!CheckUIClick()) {
						attackScript.DashAttack();
						dashChargeup = 0;
					}
				}
			}

			if (Input.GetKeyDown(KeyCode.Mouse0) && dashChargeup < dashChargeupTime) {
				if (!CheckUIClick()) {
					dashChargeup = 0;
				}
			}
		}
		else {
			dashChargeup = 0;
		}
	}

	private bool CheckUIClick() {
		RaycastHit2D hit = Physics2D.Raycast((Input.mousePosition), Vector2.zero);

		if(hit.collider == null) {
			return false;
		}
		else if (hit.collider.gameObject.layer == 5) {
			return true;
		}
		else {
			return false;
		}
	}

	private void Build() {
		if(Input.GetKeyDown(KeyCode.Mouse0)) {
			buildScript.PlaceObject();
		}
	}

	private void DestroyObject() {
		buildScript.DestroyObject();
	}
}