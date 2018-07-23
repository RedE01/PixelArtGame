using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : StorageContainer {

	public bool interactable;

	void Update() {
		if(interactable) {
			if(Input.GetButtonDown("Interact")) {
				OpenChest();
			}
		}
	}

	void OpenChest() {
		Debug.Log("OH SHIT");
	}
}
