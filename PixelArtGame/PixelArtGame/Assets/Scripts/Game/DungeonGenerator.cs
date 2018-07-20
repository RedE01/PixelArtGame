using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

	public GameObject roomPrefab;
	public GameObject startRoom;
	public GameObject DoorLPrefab, DoorRPrefab, DoorTPrefab, DoorBPrefab;
	[Space]
	public CameraScript cameraScript;
	public Vector2 roomStartPos;
	public Transform player;

	[HideInInspector]
	public Transform cameraTarget;

	List<GameObject> Rooms = new List<GameObject>();
	Vector2 currentRoomPos;
	Vector2 roomSize;
	Vector2Int[] directions = new Vector2Int[4];

	void Start() {
		Rooms.Add(startRoom);
		currentRoomPos = startRoom.transform.position;
		cameraTarget = startRoom.transform;

		roomSize = roomPrefab.GetComponent<SpriteRenderer>().size;

		directions[0] = Vector2Int.left;
		directions[1] = Vector2Int.up;
		directions[2] = Vector2Int.right;
		directions[3] = Vector2Int.down;
	}

	public void GoToRoom(Vector2Int dir) {
		Vector2 pos = GetRoomPosAtDirection(dir);
		GameObject go = CheckForRoom(pos);

		if (go == null) {
			GenerateRoom(dir);
		}
		else {
			SetRoom(go, pos, dir);
		}
	}

	GameObject CheckForRoom(Vector2 pos) {
		foreach (GameObject go in Rooms) {
			if((Vector2)go.transform.position == pos) {
				return go;
			}
		}
		return null;
	}

	void GenerateRoom(Vector2Int dir) {
		Vector2 newRoomPos = GetRoomPosAtDirection(dir);

		GameObject room = Instantiate(roomPrefab, newRoomPos, Quaternion.identity);
		Rooms.Add(room);
		SetRoom(room, newRoomPos, dir);

		for(int i = 0; i < directions.Length; i++) {
			GameObject r = CheckForRoom(GetRoomPosAtDirection(directions[i]));

			if(r != null) {
				//Debug.Log("Room in direction: " + directions[i]);
				if(r.transform.Find(GetDoorParent(directions[i] * -1)).childCount > 0) {
					//Debug.Log("OH shit");
					CreateDoor(room, directions[i]);
				}
			}
			else {
				if (Random.Range(0f, 1f) < .5f)
					CreateDoor(room, directions[i]);
			}
		}

	}

	void CreateDoor(GameObject room, Vector2Int dir) {
		GameObject door = GetDoor(dir);
		string parent = GetDoorParent(dir);

		Transform Parent = room.transform.Find(parent);
		Instantiate(door, Parent);
		Parent.GetComponent<BoxCollider2D>().enabled = false;
	}

	Vector2 GetRoomPosAtDirection(Vector2Int dir) {
		return currentRoomPos + new Vector2(dir.x * roomSize.x, dir.y * roomSize.y);
	}

	GameObject GetDoor(Vector2Int dir) {
		if (dir == Vector2Int.left) return DoorLPrefab;
		else if (dir == Vector2Int.right) return DoorRPrefab;
		else if (dir == Vector2Int.up) return DoorTPrefab;
		else return DoorBPrefab;
	}

	string GetDoorParent(Vector2Int dir) {
		if (dir == Vector2Int.left) return "DoorParentL";
		else if (dir == Vector2Int.right) return "DoorParentR";
		else if (dir == Vector2Int.up) return "DoorParentT";
		else return "DoorParentB";
	}

	void SetRoom(GameObject room, Vector2 playerPos, Vector2Int dir) {
		cameraScript.target = room.transform;
		currentRoomPos = room.transform.position;

		playerPos -= roomStartPos * dir;
		playerPos.y -= 1;
		player.position = playerPos;
	}
}
