using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCollider : MonoBehaviour {

	Attack attackScript;

	void Start() {
		attackScript = GetComponent<Attack>();
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Enemy")) {
			attackScript.enemiesInRange.Add(collision.gameObject.GetComponent<Enemy>());
		}
		if (collision.gameObject.CompareTag("Chest")) {
			collision.gameObject.GetComponent<Chest>().interactable = true;
		}
		if (collision.gameObject.CompareTag("JobSign")) {
			collision.gameObject.GetComponent<JobSign>().interactable = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Enemy")) {
			attackScript.enemiesInRange.Remove(collision.gameObject.GetComponent<Enemy>());
		}
		if (collision.gameObject.CompareTag("Chest")) {
			collision.gameObject.GetComponent<Chest>().interactable = false;
		}
		if (collision.gameObject.CompareTag("JobSign")) {
			collision.gameObject.GetComponent<JobSign>().interactable = false;
		}
	}
}
