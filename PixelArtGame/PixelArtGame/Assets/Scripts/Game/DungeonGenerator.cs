using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

	public GameObject roomPrefab;
	public GameObject startRoom;
	public Vector2 roomSize;
	public CameraScript cameraScript;

	[HideInInspector]
	public Transform cameraTarget;

	List<GameObject> Rooms = new List<GameObject>();
	Vector2 currentRoomPos;

	void Start() {
		Rooms.Add(startRoom);
		currentRoomPos = startRoom.transform.position;
		cameraTarget = startRoom.transform;

		roomSize *= 2f;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.O)) {
			if (!CheckForRoom(Vector2Int.up)) {
				GenerateRoom(Vector2Int.up);
			}
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			if (!CheckForRoom(Vector2Int.left)) {
				GenerateRoom(Vector2Int.left);
			}
		}
		if (Input.GetKeyDown(KeyCode.P)) {
			if (!CheckForRoom(Vector2Int.right)) {
				GenerateRoom(Vector2Int.right);
			}
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			if (!CheckForRoom(Vector2Int.down)) {
				GenerateRoom(Vector2Int.down);
			}
		}
	}

	bool CheckForRoom(Vector2Int dir) {
		Vector2 pos = currentRoomPos + new Vector2(dir.x * roomSize.x, dir.y * roomSize.y);

		foreach (GameObject go in Rooms) {
			if((Vector2)go.transform.position == pos) {
				SetRoom(go);

				return true;
			}
		}
		return false;
	}

	void GenerateRoom(Vector2Int dir) {
		Vector2 newRoomPos = currentRoomPos + new Vector2(dir.x * roomSize.x, dir.y * roomSize.y);

		GameObject room = Instantiate(roomPrefab, newRoomPos, Quaternion.identity);
		Rooms.Add(room);
		SetRoom(room);
	}

	void SetRoom(GameObject room) {
		cameraScript.target = room.transform;
	}
}
