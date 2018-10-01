using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSign : MonoBehaviour {

	public Player playerScript;
	public JobSignUI jobSignUI;

	[HideInInspector]
	public bool interactable = false;

	void Update() {
		if (interactable) {
			if (Input.GetButtonDown("Interact") && playerScript.playerState != Player.PlayerState.Inventory) {
				jobSignUI.OpenSign(playerScript);
			}
		}
	}

}
