using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MovableObject {

	public enum EnemyState {
		Idle,
		Chasing,
		Attacking
	}
	public EnemyState enemyState = EnemyState.Idle;

	public int health, damageKnockback;
	public float normalSpeed, chasingRange, attackRange, attackKnockback, idleWalkDistance, hitTime;
	public GameObject pickup;
	public Item drop;

	protected float speed;
	protected GameObject player;
	protected Player playerScript;
	protected Vector2 target;
	protected Rigidbody2D rb;
	protected Vector2 movement;

	float damageCounter = 0;

	protected override void Start() {
		speed = normalSpeed;
		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player>();
		rb = GetComponent<Rigidbody2D>();

		base.Start();
	}

	void Update() {
		switch (enemyState) {
			case EnemyState.Idle:
				SetRandomTarget(idleWalkDistance);
				MoveToTarget();
				speed = normalSpeed * 0.5f;

				if (Vector2.Distance(transform.position, player.transform.position) < chasingRange) {
					enemyState = EnemyState.Chasing;
					break;
				}
				if (Vector2.Distance(transform.position, target) < .5f) {
					SetRandomTarget(idleWalkDistance);
				}
				break;
			case EnemyState.Chasing:
				target = player.transform.position;
				MoveToTarget();
				speed = normalSpeed;

				if(Vector2.Distance(transform.position, target) < attackRange) {
					enemyState = EnemyState.Attacking;
				}
				if (Vector2.Distance(transform.position, target) > chasingRange) {
					enemyState = EnemyState.Idle;
				}
				break;
			case EnemyState.Attacking:
				movement = Vector2.zero;

				damageCounter += Time.deltaTime;

				if(damageCounter > hitTime) {
					playerScript.Damage(10, transform.position, attackKnockback);
					damageCounter = 0;
				}

				if (Vector2.Distance(transform.position, player.transform.position) > attackRange) {
					enemyState = EnemyState.Chasing;
					damageCounter = 0;
				}
				break;
		}
	}

	void FixedUpdate() {
		rb.AddForce(movement * speed);
	}

	public bool Damage(Vector2 attackPosition, int damageAmmount) {
		health -= damageAmmount;
		Vector2 dmgVector = new Vector2(transform.position.x - attackPosition.x, transform.position.y - attackPosition.y).normalized;
		rb.AddForce(dmgVector * damageKnockback, ForceMode2D.Impulse);

		if(health <= 0) {
			GameObject p = Instantiate(pickup, transform.position, Quaternion.identity);
			p.GetComponent<Pickup>().item = drop;
			Destroy(gameObject, .5f);
			return true;
		}
		return false;
	}

	void MoveToTarget() {
		movement = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
	}

	void SetRandomTarget(float maxDist) {
		target.x = Random.Range(0, maxDist) + transform.position.x - maxDist * 0.5f;
		target.y = Random.Range(0, maxDist) + transform.position.y - maxDist * 0.5f;
	}

	protected override SpriteRenderer SetRenderer() {
		return GetComponent<SpriteRenderer>();
	}
}