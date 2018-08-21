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
	protected int attackHash, movingHash;
	protected Animator animator;
	protected int xMoveHash, yMoveHash, deadHash;

	private float idleSpeedMultiplier = 0.1f;


	public void Start() {
		speed = normalSpeed * idleSpeedMultiplier;
		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player>();
		rb = GetComponent<Rigidbody2D>();
		target = transform.position;
		animator = GetComponent<Animator>();

		attackHash = Animator.StringToHash("Attack");
		movingHash = Animator.StringToHash("Moving");
		xMoveHash = Animator.StringToHash("MoveX");
		yMoveHash = Animator.StringToHash("MoveY");
		deadHash = Animator.StringToHash("Dead");
	}

	override public void Update() {
		switch (enemyState) {
			case EnemyState.Idle:
				speed = normalSpeed * idleSpeedMultiplier;
				if (Vector2.Distance(transform.position, player.transform.position) < chasingRange) {
					animator.SetBool(movingHash, true);
					enemyState = EnemyState.Chasing;
					speed = normalSpeed;
				}
				break;

			case EnemyState.Chasing:
			animator.SetFloat(xMoveHash, movement.x);
			animator.SetFloat(yMoveHash, movement.y);
				if(Vector2.Distance(transform.position, target) < attackRange) {
					animator.SetBool(attackHash, true);
					enemyState = EnemyState.Attacking;
				}
				if (Vector2.Distance(transform.position, target) > chasingRange) {
					animator.SetBool(movingHash, false);
					enemyState = EnemyState.Idle;
				}
				break;

			case EnemyState.Attacking:
				if (Vector2.Distance(transform.position, player.transform.position) > attackRange) {
					animator.SetBool(attackHash, false);
					animator.SetBool(movingHash, true);
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
			animator.SetTrigger(deadHash);
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

		Vector2 lookDir = target - (Vector2)transform.position;
		animator.SetFloat(xMoveHash, lookDir.x);
		animator.SetFloat(yMoveHash, lookDir.y);
	}

	void SetSpeed(int spdMultiplier) {
		speed = normalSpeed * spdMultiplier;
	}
}