using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MovableObject {

	public enum EnemyState {
		Idle,
		Chasing,
		Attacking,
		Dead
	}
	[HideInInspector]
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
	protected float damageCounter = 0;
	protected int attackHash, idleHash, chaseHash;
	protected Animator animator;


	public void Start() {
		speed = normalSpeed;
		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player>();
		rb = GetComponent<Rigidbody2D>();
		target = transform.position;
		animator = GetComponent<Animator>();

		animator.SetBool(idleHash, true);
	}

	override public void Update() {
		switch (enemyState) {
			case EnemyState.Idle:
				if (Vector2.Distance(transform.position, player.transform.position) < chasingRange) {
					animator.SetBool(idleHash, false);
					animator.SetBool(chaseHash, true);
					enemyState = EnemyState.Chasing;
				}
				break;

			case EnemyState.Chasing:
				if(Vector2.Distance(transform.position, target) < attackRange) {
					animator.SetBool(chaseHash, false);
					animator.SetBool(attackHash, true);
					enemyState = EnemyState.Attacking;
				}
				if (Vector2.Distance(transform.position, target) > chasingRange) {
					animator.SetBool(chaseHash, false);
					animator.SetBool(idleHash, true);
					enemyState = EnemyState.Idle;
				}
				break;

			case EnemyState.Attacking:
				if (Vector2.Distance(transform.position, player.transform.position) > attackRange) {
					animator.SetBool(attackHash, false);
					animator.SetBool(chaseHash, true);
					enemyState = EnemyState.Chasing;
					damageCounter = 0;
				}
				break;
		}
		base.Update();
	}

	void FixedUpdate() {
		rb.AddForce(movement * speed);
	}

	public void Patroll() {
		if (Vector2.Distance(transform.position, target) < 0.05f) {
			SetRandomTarget(idleWalkDistance);
		}
		MoveToTarget();
	}

	public void ChaseTarget() {
		target = player.transform.position;
		MoveToTarget();
	}

	public void AttackTarget() {
		movement = Vector2.zero;

		damageCounter += Time.deltaTime;
		if (damageCounter > hitTime) {
			playerScript.Damage(10, transform.position, attackKnockback);
			damageCounter = 0;
		}
	}

	public bool Damage(Vector2 attackPosition, int damageAmmount) {
		health -= damageAmmount;
		Vector2 dmgVector = new Vector2(transform.position.x - attackPosition.x, transform.position.y - attackPosition.y).normalized;
		rb.AddForce(dmgVector * damageKnockback, ForceMode2D.Impulse);

		if(health <= 0) {
			if (enemyState != EnemyState.Dead) {
				GameObject p = Instantiate(pickup, transform.position, Quaternion.identity);
				p.GetComponent<Pickup>().item = drop;
				Destroy(gameObject, .5f);
			}
			enemyState = EnemyState.Dead;
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

	public void SetSpeed(float speedMultiplier) {
		speed = normalSpeed * speedMultiplier;
	}
}