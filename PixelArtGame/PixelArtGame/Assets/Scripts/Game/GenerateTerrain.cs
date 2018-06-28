using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTerrain : MonoBehaviour {

	public Tilemap dirtTilemap;
	public Tilemap groundTilemap;
	public GameObject tree;

	public Tile[] dirtTiles;
	public Tile[] grassTiles;

	readonly int chunkWidth = 10;
	readonly int activeChunksRadius = 2;
	Transform player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;

		GenerateAroundPlayer();
	}

	void Update() {
		GenerateAroundPlayer();
	}

	void GenerateAroundPlayer() {
		for (int x = -activeChunksRadius; x < activeChunksRadius; x++) {
			for (int y = -activeChunksRadius; y < activeChunksRadius; y++) {
				Vector3Int chunk = new Vector3Int(Mathf.RoundToInt(player.position.x / chunkWidth) + x, Mathf.RoundToInt(player.position.y / chunkWidth) + y, 0);
				if (dirtTilemap.GetTile(chunk * chunkWidth) == null) {
					GenerateChunk(dirtTilemap, dirtTiles, chunk, 90);
					GenerateChunk(groundTilemap, grassTiles, chunk, 70);
				}
			}
		}
	}

	void GenerateTrees() {

	}

	void GenerateChunk(Tilemap tilemap, Tile[] tileset, Vector3Int chunk, int baseTilePercent) {
		Vector3Int pos = new Vector3Int(chunk.x * chunkWidth, chunk.y * chunkWidth, 0);
		tilemap.SetTile(new Vector3Int(pos.x + chunkWidth - 1, pos.y + chunkWidth - 1, 0), tileset[0]);
		tilemap.BoxFill(pos, tileset[0], pos.x, pos.y, pos.x + chunkWidth - 1, pos.y + chunkWidth - 1);
		for (int x = 0; x < chunkWidth; x++) {
			for (int y = 0; y < chunkWidth; y++) {
				float rand = Random.Range(0, 100);
				if (rand > baseTilePercent) {
					float max = 100 - baseTilePercent;
					rand -= baseTilePercent;
					float result = ((float)rand / (float)max) * (dirtTiles.Length - 1);
					int tile = Mathf.RoundToInt(result);

					tilemap.SetTile(new Vector3Int(pos.x + x, pos.y + y, 0), tileset[tile]);
				}
			}
		}
	}

}
