using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

	public GameObject roomPrefab;
	public GameObject startRoom;
	public CameraScript cameraScript;
	public Vector2 roomStartPos;
	public Transform player;

	[HideInInspector]
	public Transform cameraTarget;

	List<GameObject> Rooms = new List<GameObject>();
	Vector2 currentRoomPos;
	Vector2 roomSize;

	void Start() {
		Rooms.Add(startRoom);
		currentRoomPos = startRoom.transform.position;
		cameraTarget = startRoom.transform;

		roomSize = roomPrefab.GetComponent<SpriteRenderer>().size;
	}

	public void GoToRoom(Vector2Int dir) {
		if (!CheckForRoom(dir)) {
			GenerateRoom(dir);
		}
	}

	bool CheckForRoom(Vector2Int dir) {
		Vector2 pos = currentRoomPos + new Vector2(dir.x * roomSize.x, dir.y * roomSize.y);

		foreach (GameObject go in Rooms) {
			if((Vector2)go.transform.position == pos) {
				SetRoom(go, pos, dir);

				return true;
			}
		}
		return false;
	}

	void GenerateRoom(Vector2Int dir) {
		Vector2 newRoomPos = currentRoomPos + new Vector2(dir.x * roomSize.x, dir.y * roomSize.y);

		GameObject room = Instantiate(roomPrefab, newRoomPos, Quaternion.identity);
		Rooms.Add(room);
		SetRoom(room, newRoomPos, dir);
	}

	void SetRoom(GameObject room, Vector2 playerPos, Vector2Int dir) {
		cameraScript.target = room.transform;
		currentRoomPos = room.transform.position;

		playerPos -= roomStartPos * dir;
		playerPos.y -= 1;
		player.position = playerPos;
	}
}
