using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Skeleton : Enemy {

	public float attackDamage, attackAngle;

	Vector2 targetDir;

	new void Start() {
		attackHash = Animator.StringToHash("SkeletonAttack001");
		idleHash = Animator.StringToHash("SkeletonIdle");
		chaseHash = Animator.StringToHash("SkeletonChasing");

		base.Start();
	}

	public override void Update() {
		base.Update();

		switch (enemyState) {
			case EnemyState.Idle:
				Patroll();
				break;
			case EnemyState.Chasing:
				ChaseTarget();
				break;
			case EnemyState.Attacking:
				Attack();
				break;
		}
	}

	void Attack() {
		if(!animator.GetBool(attackHash)) {
			animator.SetBool(attackHash, true);
			targetDir = player.transform.position - transform.position;
		}
	}

	public void DamageTarget() {
		animator.SetBool(attackHash, false);
		if(Vector2.Angle(targetDir, player.transform.position - transform.position) < attackAngle && Vector2.Distance(transform.position, player.transform.position) < attackRange) {
			playerScript.Damage(attackDamage, transform.position, attackKnockback);
		}
	}
}
