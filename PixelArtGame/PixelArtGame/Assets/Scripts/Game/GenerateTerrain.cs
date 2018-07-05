using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTerrain : MonoBehaviour {

	public Tilemap dirtTilemap;
	public Tilemap groundTilemap;
	public Tilemap hillsTilemap;
	public GameObject tree;
	public Transform treeParent;
	public float treePercent;

	public Tile[] dirtTiles;
	public Tile[] grassTiles;
	public Tile[] hillTiles;

	readonly int chunkWidth = 10;
	readonly int activeChunksRadius = 2;
	readonly int worldSize = 500;
	int seed = 0;
	float scale = 0.75f;
	Transform player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;

		GenerateAroundPlayer();
	}

	void Update() {
		GenerateAroundPlayer();

		//GenerateHills(new Vector3Int(6, -2, 0));

	}

	void GenerateAroundPlayer() {
		for (int x = -activeChunksRadius; x < activeChunksRadius; x++) {
			for (int y = -activeChunksRadius; y < activeChunksRadius; y++) {
				Vector3Int chunk = new Vector3Int(Mathf.RoundToInt(player.position.x / chunkWidth) + x, Mathf.RoundToInt(player.position.y / chunkWidth) + y, 0);
				if (dirtTilemap.GetTile(chunk * chunkWidth) == null) {
					GenerateChunk(dirtTilemap, dirtTiles, chunk, 90);
					GenerateChunk(groundTilemap, grassTiles, chunk, 75);

					GenerateHills(chunk);
					
					GenerateTrees(chunk);
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
						//GameObject wa =
						Instantiate(tree, treePos[treeNumber], Quaternion.identity, treeParent);
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

	void GenerateHills(Vector3Int chunk) {
		Vector3Int pos = new Vector3Int(chunk.x * chunkWidth, chunk.y * chunkWidth, 0);
		for (int x = 0; x < chunkWidth; x++) {
			for (int y = 0; y < chunkWidth; y++) {
				float xCoord = (float)x / (float)chunkWidth * (float)scale + (float)chunk.x * scale + worldSize;
				float yCoord = (float)y / (float)chunkWidth * (float)scale + (float)chunk.y * scale + worldSize;

				float height = Mathf.PerlinNoise(xCoord, yCoord);

				if(height > 0.6f) {
					bool below = false;
					bool right = false;
					bool left = false;
					bool above = false;

					if (CheckPerlinNoiseAtPos(xCoord, yCoord, 0, -1, 0.6f)) {// checks below
						below = true;
					}
					if (CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 0, 0.6f)) {// checks right
						right = true;
					}
					if (CheckPerlinNoiseAtPos(xCoord, yCoord, 0, 1, 0.6f)) {// checks above
						above = true;
					}
					if (CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 0, 0.6f)) {// checks left
						left = true;
					}

					if (below && !right && !left && !above) { //below 
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, -1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[20]);
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[18]);
						}
						else if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, -1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[21]);
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[19]);
						}
						else {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[10]);
						}
					}

					else if (!below && right && !left && !above) { //right
						hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[3]);
					}

					else if (!below && !right && left && !above) { //left
						hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[0]);
					}

					else if (!below && !right && !left && above) {//above
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[22]);
						}
						else if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[23]);
						}
						else {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[6]);
						}
					}

					else if (!below && !right && left && above) {//left && above
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[12]);
						}
						else {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[7]);
						}
					}

					else if (!below && right && !left && above) {//right && above
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[13]);
						}
						else {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[15]);
						}
					}

					else if (below && !right && left && !above) {//left && below
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, -1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[16]);
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y -1, 0), hillTiles[18]);
						}
						else {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[8]);
						}
					}

					else if (below && right && !left && !above) {//right && below
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, -1, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[17]);
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y - 1, 0), hillTiles[19]);
						}
						else {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), hillTiles[11]);
						}
					}
					else if(below && above) {
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, 0, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x -1, y + pos.y, 0), hillTiles[3]);
						}
						else if (!CheckPerlinNoiseAtPos(xCoord, yCoord, 1, 0, 0.6f)) {
							hillsTilemap.SetTile(new Vector3Int(x + pos.x + 1, y + pos.y, 0), hillTiles[0]);
						}
					}
					else if(right && left) {
						if (!CheckPerlinNoiseAtPos(xCoord, yCoord, -1, -1, 0.6f)) {
						}
					}
					else {
						groundTilemap.SetTile(new Vector3Int(x + pos.x, y + pos.y, 0), grassTiles[0]);
					}
				}
			}
		}
	}

	bool CheckPerlinNoiseAtPos(float xCoord, float yCoord, int xOffset, int yOffset, float percentage) {
		return Mathf.PerlinNoise(xCoord + (scale / (float)chunkWidth) * (float)xOffset, yCoord + (scale / (float)chunkWidth) * (float)yOffset) <= percentage;
	} 

}
