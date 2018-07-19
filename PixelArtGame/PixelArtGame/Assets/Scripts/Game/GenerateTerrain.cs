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

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		seed = Random.Range(0, 10000);

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
					GenerateChunk(groundTilemap, grassTiles, chunk, 75);

					GenerateHills(hillsTilemap1, chunk, 0.45f);
					GenerateHills(hillsTilemap2, chunk, 0.6f);
					GenerateHills(hillsTilemap3, chunk, 0.7f);

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
						//if (groundTilemap.GetTile(new Vector3Int(pos.x + x, pos.y + y, 0)) == null) {
						//	wa.name = "RIP";
						//}
						//else {

						//}
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

	void GenerateHills(Tilemap tilemap, Vector3Int chunk, float checkOver) {
		Vector3Int pos = new Vector3Int(chunk.x * chunkWidth, chunk.y * chunkWidth, 0);

		for (int x = 0; x < chunkWidth; x++) {
			for (int y = 0; y < chunkWidth; y++) {
				float xCoord = (float)x / (float)chunkWidth * (float)scale + (float)chunk.x * scale + worldSize + seed;
				float yCoord = (float)y / (float)chunkWidth * (float)scale + (float)chunk.y * scale + worldSize + seed;

				if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 0, 0, checkOver, x, y, pos)) {
					bool below = false;
					bool right = false;
					bool left = false;
					bool above = false;

					if (CheckPerlinNoiseAtPos(xCoord, yCoord, 0, -1, checkOver, x, y, pos)) {// checks below
						below = true;
					}
					if (CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 0, checkOver, x, y, pos)) {// checks right
						right = true;
					}
					if (CheckPerlinNoiseAtPos(xCoord, yCoord, 0, 1, checkOver, x, y, pos)) {// checks above
						above = true;
					}
					if (CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 0, checkOver, x, y, pos)) {// checks left
						left = true;
					}

					SetHillTile(tilemap, pos, x, y, xCoord, yCoord, below, right, left, above, checkOver);
				}
			}

		}
	}

	bool CheckPerlinNoiseAtPos(float xCoord, float yCoord, int xOffset, int yOffset, float checkOver, int x, int y, Vector3Int pos) {
		int xAbs = Mathf.Abs(x + xOffset + pos.x);
		int yAbs = Mathf.Abs(y + yOffset + pos.y);

		float limiter = 1;
		if (xAbs < startArea.x + chunkWidth && yAbs < startArea.y + chunkWidth) {
			limiter = (xAbs + startArea.x > yAbs + startArea.y) ? (float)(xAbs - startArea.x) : (float)(yAbs - startArea.y);
			limiter = (limiter) / (chunkWidth);
			limiter = Mathf.Clamp(limiter + 0.2f, 0, 1);
		}

		return (Mathf.PerlinNoise(xCoord + (scale / (float)chunkWidth) * (float)xOffset, yCoord + (scale / (float)chunkWidth) * (float)yOffset)) * limiter <= checkOver;
	}

	void SetHillTile(Tilemap tilemap, Vector3Int pos, int x, int y, float xCoord, float yCoord, bool below, bool right, bool left, bool above, float checkOver) {
		if (below && !right && !left && !above) { //below 
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, -1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[20]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[18]);
			}
			else if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, -1, checkOver, x, y, pos)) {
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
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[22]);
			}
			else if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[23]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[6]);
			}
		}

		else if (!below && !right && left && above) {//left && above
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[12]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[7]);
			}
		}

		else if (!below && right && !left && above) {//right && above
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[13]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[15]);
			}
		}

		else if (below && !right && left && !above) {//left && below
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, -1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[16]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[18]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[8]);
			}
		}

		else if (below && right && !left && !above) {//right && below
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, -1, checkOver, x, y, pos)) { //top left diagonal
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[17]);
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[19]);
			}
			else {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[11]);
			}
		}

		else if (below && above) { //below and above
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 0, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x - 1, y + pos.y, 0), hillTiles[3]);
			}
			else /*if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 0, checkOver))*/ {
				tilemap.SetTile(new Vector3Int(x + pos.x + 1, y + pos.y, 0), hillTiles[0]);
			}
		}

		else if (right && left) { //right and left
			tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), dirtTiles[1]);
			if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 0, 1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[1]);
			}
			else if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 0, -1, checkOver, x, y, pos)) {
				tilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[2]);
			}
		}

		//if (x + pos.x < -43 && x + pos.x > -47 && y + pos.y > -13 && y + pos.y < -9)
		//	Debug.Log(new Vector3Int(x + pos.x, y + pos.y, 0) + " : " + Mathf.PerlinNoise(xCoord + (scale / (float)chunkWidth) * 0, yCoord + (scale / (float)chunkWidth) * 0));
	}
}