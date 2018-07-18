using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovableObject {

	public enum PlayerState : short {
		Default,
		Menu,
		Inventory,
		Destroying,
		Dashing,
		Attacking
	}
	public PlayerState playerState = PlayerState.Default;

	public Animator animator;
	public float speed, dashChargeupTime;

	[HideInInspector]
	public Vector2 facing;

	HealthUI healthbar;
	Vector2 movement;
	Rigidbody2D rb;
	Attack attackScript;
	DestroyObject destroyObjectScript;
	PlayerHand playerHandScript;
	CameraScript cameraScript;
	float dashChargeup;
	int jumpHash = Animator.StringToHash("Jump");

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		attackScript = GetComponentInChildren<Attack>();
		cameraScript = Camera.main.GetComponent<CameraScript>();
		destroyObjectScript = GetComponentInChildren<DestroyObject>();
		healthbar = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<HealthUI>();
		playerHandScript = GetComponentInChildren<PlayerHand>();
	}

	new void Update() {
		if (Input.GetKeyDown(KeyCode.K)) SceneManager.LoadScene("Overworld", LoadSceneMode.Single);

		facing = new Vector2(GameManager.instance.mousePos.x - transform.position.x, GameManager.instance.mousePos.y - transform.position.y).normalized;
		movement = Vector2.zero;

		if (playerState == PlayerState.Default) {
			float facingRad = Mathf.Atan2(facing.x, facing.y);
			attackScript.transform.rotation = Quaternion.AngleAxis(-facingRad * Mathf.Rad2Deg, new Vector3(0, 0, 1));
		}

		switch (playerState) {
			case PlayerState.Default:
				Movement();

				switch (playerHandScript.GetItemType(playerHandScript.item)) {
					case Item.ItemType.Axe:
						if(!destroyObjectScript.DestroyTarget()) {
							Attack();
						}
						break;
					case Item.ItemType.Item:
						Attack();
						break;
				}

				break;
			case PlayerState.Destroying:
				Movement();
				break;
		}
		base.Update();
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
		movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); ;

		if (movement.magnitude > 1) {
			movement = movement.normalized;
		}
		movement *= speed;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (input.x != 0 || input.y != 0) {
			animator.SetBool("PlayerMoving", true);

			animator.SetFloat("MoveX", input.x);
			animator.SetFloat("MoveY", input.y);
		}
		else {
			animator.SetBool("PlayerMoving", false);
		}

		if (Input.GetButtonDown("Jump")) {
			animator.SetTrigger(jumpHash);
		}
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

	void ToggleHillCollision(int oneForTrueZeroForFalse) {
		bool collideWithHills = (oneForTrueZeroForFalse == 0) ? false : true;
		Physics2D.IgnoreLayerCollision(0, 8, collideWithHills);
	}

	void EnterPortal() {
		animator.SetBool("CollidingWithPortal", false);
		SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Portal")) {
			animator.SetBool("CollidingWithPortal", true);
		}
	}

	void OnTriggerExit2D(Collider2D collision) {
		if(collision.CompareTag("Portal")) {
			animator.SetBool("CollidingWithPortal", false);
		}
	}
	//private void Build() {
	//	if(Input.GetKeyDown(KeyCode.Mouse0)) {
	//		buildScript.PlaceObject();
	//	}
	//}
}