using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTerrain : MonoBehaviour {

	public Tilemap dirtTilemap;
	public Tilemap groundTilemap;
	public Tilemap hillsTilemap1;
	public Tilemap hillsTilemap2;
	public Tilemap hillsTilemap3;
	public GameObject tree;
	public Transform treeParent;
	public float treePercent;

	public Tile[] dirtTiles;
	public Tile[] grassTiles;
	public Tile[] hillTiles;

	readonly int chunkWidth = 10;
	readonly int activeChunksRadius = 3;
	readonly int worldSize = 500;
	readonly Vector2Int startArea = new Vector2Int(20, 10);
	int seed;
	float scale = 0.75f;
	Transform player;
	Vector3Int chunk = Vector3Int.zero;


	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		seed = Random.Range(0, 10000);
		Debug.Log(seed);

		GenerateAroundPlayer();
	}

	void Update() {
		GenerateAroundPlayer();
	}

	void GenerateAroundPlayer() {
		for (int x = -activeChunksRadius; x < activeChunksRadius; x++) {
			for (int y = -activeChunksRadius; y < activeChunksRadius; y++) {
				chunk = new Vector3Int(Mathf.RoundToInt(player.position.x / chunkWidth) + x, Mathf.RoundToInt(player.position.y / chunkWidth) + y, 0);
				if (dirtTilemap.GetTile(chunk * chunkWidth) == null) {
					GenerateChunk(dirtTilemap, dirtTiles, chunk, 90);
					GenerateChunk(groundTilemap, grassTiles, chunk, 75);

					GenerateHills(hillsTilemap1, chunk, 0.45f, dirtTiles[0]);
					GenerateHills(hillsTilemap2, chunk, 0.6f, dirtTiles[1]);
					GenerateHills(hillsTilemap3, chunk, 0.7f, dirtTiles[2]);

					GenerateTrees(chunk);
					return;
				}
			}
		}
	}

	void GenerateTrees(Vector3Int chunk) {
		Vector3Int pos = new Vector3Int(chunk.x * chunkWidth, chunk.y * chunkWidth, 0);
		Vector2[] treePos = new Vector2[10];
		int treeNumber = 0;
		for (int x = 0; x < chunkWidth; x++) {
			for (int y = 0; y < chunkWidth; y++) {
				float rand = Random.Range(0, 100);

				if (rand < treePercent && treeNumber < 10) {
					treePos[treeNumber] = new Vector2(pos.x + x + 0.5f, pos.y + y);
					bool test = true;

					for (int i = 0; i < treePos.Length; i++) {
						if (treeNumber != i && treePos[i] != Vector2.zero && Vector2.Distance(treePos[treeNumber], treePos[i]) < 2f) {
							test = false;
						}
					}
					if (test) {
						Vector3Int tilePos = new Vector3Int(pos.x + x, pos.y + y, 0);
						if (hillsTilemap1.GetTile(tilePos) == null) {
							Instantiate(tree, treePos[treeNumber], Quaternion.identity, treeParent);
						}
					}
					treeNumber++;
				}
			}
		}
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
					float result = ((float)rand / (float)max) * (tileset.Length - 1);
					int tile = Mathf.RoundToInt(result);

					tilemap.SetTile(new Vector3Int(pos.x + x, pos.y + y, 0), tileset[tile]);
				}

			}
		}
	}

	void GenerateHills(Tilemap tilemap, Vector3Int chunk, float checkOver, Tile t) {
		Vector3Int pos = new Vector3Int(chunk.x * chunkWidth, chunk.y * chunkWidth, 0);

		for (int x = 0; x < chunkWidth; x++) {
			for (int y = 0; y < chunkWidth; y++) {
				
				if (!CheckPerlinNoiseAtPos(x + pos.x, y + pos.y, checkOver)) {
					bool below = false;
					bool right = false;
					bool left = false;
					bool above = false;

					if (CheckPerlinNoiseAtPos(x + pos.x, y + pos.y - 1, checkOver)) {// checks below
						below = true;
					}
					if (CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y, checkOver)) {// checks right
						right = true;
					}
					if (CheckPerlinNoiseAtPos(x + pos.x, y + pos.y + 1, checkOver)) {// checks above
						above = true;
					}
					if (CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y, checkOver)) {// checks left
						left = true;
					}
					
					if ((below || right || above || left)) {
						SetHillTile(tilemap, pos, x, y, below, right, left, above, checkOver);
					}
				}
			}

		}
	}

	bool CheckPerlinNoiseAtPos(float worldX, float worldY, float checkOver) { //returns true if there won't be a tile at direction specified
		float perlinX = (worldX / chunkWidth) * scale + seed + worldSize;
		float perlinY = (worldY / chunkWidth) * scale + seed + worldSize;

		float xDist = Mathf.Abs(worldX);
		float yDist = Mathf.Abs(worldY);
		float multiplier = 1;
		if(xDist < startArea.x + chunkWidth && yDist < startArea.y + chunkWidth) {
			multiplier = (xDist + startArea.x > yDist + startArea.y ? xDist - startArea.x : yDist - startArea.y) / chunkWidth;
			multiplier = Mathf.Clamp(multiplier + (1.0f - checkOver) / 2.0f, 0, 1);
		}

		return Mathf.PerlinNoise(perlinX, perlinY) * multiplier <= checkOver;
	}

	void SetHillTile(Tilemap tilemap, Vector3Int pos, int x, int y, bool below, bool right, bool left, bool above, float checkOver) {
		
		if (below && !right && !left && !above) { //below 
			if (!CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y - 1, checkOver) && !CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y - 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[26]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[25]);
			}
			else if (!CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y - 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[20]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[18]);
			}
			else if (!CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y - 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[21]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[19]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[10]);
			}
		}
		else if (!below && right && !left && !above) { //right
			tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[3]);
		}

		else if (!below && !right && left && !above) { //left
			tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[0]);
		}

		else if (!below && !right && !left && above) {//above
			if (!CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y + 1, checkOver) && !CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y + 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[24]);
			}
			else if (!CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y + 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[22]);
			}
			else if (!CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y + 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[23]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[6]);
			}
		}

		else if (!below && !right && left && above) {//left && above
			if (!CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y + 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[12]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[7]);
			}
		}

		else if (!below && right && !left && above) {//right && above
			if (!CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y + 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[13]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[15]);
			}
		}

		else if (below && !right && left && !above) {//left && below
			if (!CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y - 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[16]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[18]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[8]);
			}
		}

		else if (below && right && !left && !above) {//right && below
			if (!CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y - 1, checkOver)) { //top left diagonal
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[17]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[19]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[11]);
				if (x + pos.x == 12 && y + pos.y == 31) Debug.Log("OOOH SHT");
			}
		}

		else if (below && above) { //below and above
			if (!CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y + 1, checkOver) && !CheckPerlinNoiseAtPos(x + pos.x - 1, y + pos.y - 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[28]);
			}
			else if (!CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y + 1, checkOver) && !CheckPerlinNoiseAtPos(x + pos.x + 1, y + pos.y - 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[27]);
			}
		}

		else if (right && left) { //right and left
			if (!CheckPerlinNoiseAtPos(x + pos.x, y + pos.y + 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[1]);
			}
			else if (!CheckPerlinNoiseAtPos(x + pos.x, y + pos.y - 1, checkOver)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[2]);
			}
		}

		//if (x + pos.x < -43 && x + pos.x > -47 && y + pos.y > -13 && y + pos.y < -9)
		//	Debug.Log(new Vector3Int(x + pos.x, y + pos.y, 0) + " : " + Mathf.PerlinNoise(xCoord + (scale / (float)chunkWidth) * 0, yCoord + (scale / (float)chunkWidth) * 0));
	}
}