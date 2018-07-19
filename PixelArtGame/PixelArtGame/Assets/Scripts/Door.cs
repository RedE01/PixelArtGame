using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	DungeonGenerator roomGeneratorScript;
	public Vector2Int targetRoomDirection;

	void Start() {
		roomGeneratorScript = Camera.main.GetComponent<DungeonGenerator>();
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Player")) {
			roomGeneratorScript.GoToRoom(targetRoomDirection);
		}
	}

}
