using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Build : MonoBehaviour {

	public Transform buildings;
	public GameObject buildingTilemapTemplate;
	public Tile tile;

	Tilemap tileMap;
	Player playerScript;

	void Start() {
		playerScript = GetComponent<Player>();
	}

	public void PlaceObject() {
		Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y + playerScript.yFeetPos), Mathf.FloorToInt(transform.position.z));
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

}
