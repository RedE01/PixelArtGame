using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditWorld : MonoBehaviour {

	public Transform buildings;
	public GameObject buildingTilemapTemplate;
	public Tile tile;
	public float destroyObjectTime;

	Tilemap tileMap;
	Player playerScript;
	GameObject destroyTarget;
	float destroyTimer;
	bool destroyingObject;

	void Start() {
		playerScript = transform.parent.GetComponent<Player>();
	}

	public void PlaceObject() {
		Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
		tilePos.x += Mathf.RoundToInt(playerScript.facing.x);
		tilePos.y += Mathf.RoundToInt(playerScript.facing.y);
		string tilePosYString = tilePos.y.ToString();

		if (buildings.Find(tilePosYString)) {
			tileMap = buildings.Find(tilePosYString).GetComponent<Tilemap>();
		}
		else {
			GameObject tileMapObject = Instantiate(buildingTilemapTemplate, Vector2.zero, Quaternion.identity, buildings);
			tileMapObject.name = tilePosYString;
			tileMapObject.GetComponent<TilemapRenderer>().sortingOrder = tilePos.y * GameManager.instance.sortingOrderPrecision;

			tileMap = tileMapObject.GetComponent<Tilemap>();
		}

		tileMap.SetTile(tilePos, tile);
	}

	public void DestroyObject() {
		if (Input.GetButtonDown("Mouse 0") && destroyTarget != null) {
			destroyingObject = true;
			destroyTimer = 0;
		}

		if (destroyingObject) {
			destroyTimer += Time.deltaTime;

			if (destroyTimer > destroyObjectTime) {
				destroyTarget.GetComponent<BreakableObject>().Destroy(5, 10);
				destroyTarget = null;
				destroyTimer = 0;
				destroyingObject = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.CompareTag("Object")) {
			destroyTarget = collision.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject == destroyTarget) {
			destroyTarget = null;
			destroyingObject = false;
		}
	}

}
