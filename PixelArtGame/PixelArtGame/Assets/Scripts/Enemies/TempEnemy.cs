using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : Enemy {

	override public void Update() {
		base.Update();

		switch(enemyState) {
			case EnemyState.Idle:
				Patroll();
				break;
			case EnemyState.Chasing:
				ChaseTarget();
				break;
			case EnemyState.Attacking:
				AttackTarget();
				break;
		}
	}
}
