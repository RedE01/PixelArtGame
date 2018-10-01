using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSignUI : MonoBehaviour {

	void Update() {
		if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Inventory")) {
			gameObject.SetActive(false);
		}
	}

	public void OpenSign(Player playerScript) {
		playerScript.playerState = Player.PlayerState.Inventory;
		gameObject.SetActive(true);
	}
}
