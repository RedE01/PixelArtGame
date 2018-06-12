using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public float hitTime;

	ParticleSystem attackParticleSystem;
	List<Enemy> enemiesInRange = new List<Enemy>();

	void Start() {
		attackParticleSystem = GetComponentInChildren<ParticleSystem>();
	}

	public void SwordParticles() {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 facing = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
		float facingRad = Mathf.Atan2(facing.x, facing.y);

		var partMain = attackParticleSystem.main;
		partMain.startRotation = facingRad - 2.25f; //-2.25 rad ~ -130 deg, to to fix rotation

		transform.rotation = Quaternion.AngleAxis(-facingRad * Mathf.Rad2Deg, new Vector3(0, 0, 1));

		attackParticleSystem.Play();
	}

	void HitEnemy() {
		for (int i = enemiesInRange.Count - 1; i > -1; i--) {
			if (enemiesInRange[i] == null) { //Removes enemy from list if it doesn't exist
				enemiesInRange.RemoveAt(i);
				break;
			}
			if (enemiesInRange[i].Damage(transform.position, 5)) { //Deals damage to enemy and removes from list if destroyed
				enemiesInRange.RemoveAt(i);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.CompareTag("Enemy")) {
			enemiesInRange.Add(collision.gameObject.GetComponent<Enemy>());
		}
	}

	void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Enemy")) {
			enemiesInRange.Remove(collision.gameObject.GetComponent<Enemy>());
		}
	}
}
