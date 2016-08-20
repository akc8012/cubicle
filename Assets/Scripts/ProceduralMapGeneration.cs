// TRISTAN
using UnityEngine;
using System.Collections;

// - Creates a loop placing tiles into a grid
// - Checks through placed grid, places a start and an end location
//		by splitting the grid into zones, and placing start and end 
//		locations opposite each other
// - Places specific start / end tiles at these locations
// - Creates a path from the start to the end location
// - Omitting these path tiles from the search, cycle through all 
//		tiles and place random MapTiles containing prefabs for the office

public class ProceduralMapGeneration : MonoBehaviour
{
	int scaleSize = 10;
	Vector2 floorSize;
	Vector2 tileSize = new Vector2(0.1f,0.1f); 	// width, length
	MapTile[,] mapTile;

	GameObject[,] tiles;
	public GameObject emptyTile, 
		Cube2Wall,  Cube2WallCornerTall,  Cube2WallCorner, 
		Cube3Wall,  Cube4Wall,  CubeTEST,
		CubeLeftShort, CubeLeftTall, CubeLeftDoor,

		CubeLeftShortProps1, CubeLeftShortProps2,
		CubeLeftShortProps3, CubeLeftShortProps4,
		CubeLeftShortProps5, CubeLeftShortProps6,
		CubeLeftShortProps7;

	void Start()
	{
		Initialize ();

		FindAssets ();
		CreateBasicTileLayout ();
		DefineStartEnd ();
		FillGrid ();
	}

	void Update()
	{
		
	}

	void Initialize(){

		mapTile = new MapTile[scaleSize, scaleSize];
		tiles = new GameObject[scaleSize, scaleSize];
		floorSize = new Vector2(scaleSize,scaleSize);	// width, length - always square
	}

	void FindAssets(){

		emptyTile 			= Resources.Load ("GenericModules/GenericTile") as GameObject;
		CubeTEST 			= Resources.Load ("GenericModules/CubeTEST") as GameObject;
		CubeLeftShort 		= Resources.Load ("GenericModules/CubeLeftShort") as GameObject;
		CubeLeftTall 		= Resources.Load ("GenericModules/CubeLeftTall") as GameObject;
		CubeLeftDoor		= Resources.Load ("GenericModules/CubeLeftTallDoor") as GameObject;
		Cube2Wall 			= Resources.Load ("GenericModules/Cube2Wall") as GameObject;
		Cube2WallCorner		= Resources.Load ("GenericModules/Cube2WallCorner") as GameObject;
		Cube2WallCornerTall = Resources.Load ("GenericModules/Cube2WallCornerTall") as GameObject;
		Cube3Wall			= Resources.Load ("GenericModules/Cube3Wall") as GameObject;
		Cube4Wall 			= Resources.Load ("GenericModules/Cube4Wall") as GameObject;

		CubeLeftShortProps1 = Resources.Load ("_prefabs/CubeLeftShortProps1") as GameObject;
		CubeLeftShortProps2 = Resources.Load ("_prefabs/CubeLeftShortProps2") as GameObject;
		CubeLeftShortProps3 = Resources.Load ("_prefabs/CubeLeftShortProps3") as GameObject;
		CubeLeftShortProps4 = Resources.Load ("_prefabs/CubeLeftShortProps4") as GameObject;
		CubeLeftShortProps5 = Resources.Load ("_prefabs/CubeLeftShortProps5") as GameObject;
		CubeLeftShortProps6 = Resources.Load ("_prefabs/CubeLeftShortProps6") as GameObject;
		CubeLeftShortProps7 = Resources.Load ("_prefabs/CubeLeftShortProps7") as GameObject;
	}
		
	void CreateBasicTileLayout(){

		for (int i = 0; i < floorSize.x; i++)
			for (int j = 0; j < floorSize.y; j++) {

				ReplaceCell (i, j, emptyTile, tileSize.x, tileSize.y, 0);
				mapTile[i,j] = new MapTile (new Vector2(tiles[i,j].transform.position.x, tiles[i,j].transform.position.z), false);
			}
	}

	void DefineStartEnd(){

		int randStart = 2;//(int)Random.Range (2, 4);
		Vector2 startSpot = new Vector2 (0, 0);
		Vector2 endSpot = new Vector2 (0, 0);

		switch (randStart) {
		case 0:
			randStart = (int)Random.Range (0, floorSize.x / 2);

			startSpot = new Vector2(randStart, 0);
			endSpot = new Vector2(floorSize.x - randStart -1, floorSize.y -1);

			mapTile[(int)startSpot.x, (int)startSpot.y].SetVisited (true);
			mapTile[(int)endSpot.x, (int)endSpot.y].SetVisited (true);
			break;

		case 1:
			randStart = (int)Random.Range (floorSize.x/2, floorSize.x);

			startSpot = new Vector2(randStart, 0);
			endSpot = new Vector2(floorSize.x - randStart -1, floorSize.y -1);

			mapTile[(int)startSpot.x, (int)startSpot.y].SetVisited (true);
			mapTile[(int)endSpot.x, (int)endSpot.y].SetVisited (true);
			break;

		case 2:
			randStart = (int)Random.Range (0+1, floorSize.y-1);

			startSpot = new Vector2(0, randStart);
			endSpot = new Vector2(floorSize.x -1, floorSize.y - randStart -1);

			mapTile[(int)startSpot.x, (int)startSpot.y].SetVisited (true);
			mapTile[(int)endSpot.x, (int)endSpot.y].SetVisited (true);
			break;

		case 3:
			randStart = (int)Random.Range (floorSize.y/2, floorSize.y-1);

			startSpot = new Vector2(0, randStart);
			endSpot = new Vector2(floorSize.x -1, floorSize.y - randStart -1);

			mapTile[(int)startSpot.x, (int)startSpot.y].SetVisited (true);
			mapTile[(int)endSpot.x, (int)endSpot.y].SetVisited (true);
			break;
		}

		Debug.Log ("Start: " + startSpot + ", End: " + endSpot);

		FindPath (startSpot, endSpot);
	}

	void FindPath(Vector2 start, Vector2 end){

		// choose how many turns the path will make
		int turnCount = 1;
		int varyLength = Random.Range(0,scaleSize/3) + 1;
		int previousLength = 0;
		int cutPoint;

		Debug.Log ("Number of turns: " + turnCount + " and Vary Length: " + varyLength);


		// choose random lengths of each turn for number of turns
		// lengths have to be between (previousLength) and (maxLength/turnCount)
		int[] lengths = new int[2];
		int temp = Random.Range (1, scaleSize-1);
		cutPoint = temp;
		Debug.Log ("Cut Point " + ": " + (temp));
		for (int i = 0; i < 2; i++) {
			
			lengths [i] = temp - previousLength;
			previousLength = lengths [i];
			temp = scaleSize;

			Debug.Log ("Length " + (i+1) + ": " + lengths [i]);
		}

		// TEST:  change color of emptyTiles

		// Color first length from start to turn
		// Color second length from end to turn, backwards

		for (int h = 0; h < lengths[0]; h++) {
			
			tiles[(int)start.x + h + 1, (int)start.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
			mapTile [(int)start.x + h + 1, (int)start.y].SetVisited (true);
		}
		for (int h = 0; h < lengths[1] -1; h++) {

			tiles[(int)end.x - h - 1, (int)end.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
			mapTile [(int)end.x - h - 1, (int)end.y].SetVisited (true);
		}

		// Color start and end
		tiles[(int)start.x,(int)start.y].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		mapTile[(int)start.x,(int)start.y].SetVisited (true);
		tiles[(int)end.x,(int)end.y].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		mapTile [(int)end.x, (int)end.y].SetVisited (true);

		for (int h = 0; h < Mathf.Abs(start.y - end.y); h++) {

			if (start.y - end.y < 0) {	//	if negative, Start is above End
				tiles [(int)cutPoint, (int)start.y + h].GetComponent<Renderer> ().material.SetColor ("_Color", Color.red);
				mapTile [(int)cutPoint, (int)start.y + h].SetVisited (true);
			} 
			else if (start.y - end.y > 0) {	
				tiles [(int)cutPoint, (int)start.y - h].GetComponent<Renderer> ().material.SetColor ("_Color", Color.red);
				mapTile [(int)cutPoint, (int)start.y - h].SetVisited (true);
			}
		}
	}

	void FillGrid(){
		
		for (int i = 0; i < floorSize.x; i++) {
			for (int j = 0; j < floorSize.y; j++) {
				if (!mapTile [i, j].GetVisited ()) {

					#region boundary walls
					if (i == 0 || j == 0 || i == floorSize.y - 1 || j == floorSize.y - 1 || i == 2 || j == 2 || i == 7 || j == 7) {
						
						if (i == 0) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 0);
						}
						if (j == 0) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 270);
						}
						if (j == floorSize.y - 1) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 90);
						}
						if (i == floorSize.y - 1) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 180);
						}
						if (i == 0 && i == 2) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 0);
						}
						if (j == 0 && j == 2) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 270);
						}
						if (j == floorSize.y - 1 && j == 7) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 90);
						}
						if (i == floorSize.y - 1 && i == 7) {
							ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 180);
						}
						if (i == 0 && j == 0) {
							ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 180);
						}
						if (i == 0 && j == floorSize.y - 1) {
							ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 270);
						}
						if (i == floorSize.y - 1 && j == 0) {
							ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 90);
						}
						if (i == floorSize.y - 1 && j == floorSize.y - 1) {
							ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 0);
						}

					} else {

						int rand = 1;//Random.Range (0, 3);
						switch (rand) {
						case 0:
							Destroy (tiles [i, j].gameObject);
							tiles [i, j] = (GameObject)Instantiate (Cube2Wall, new Vector3 (i * tileSize.x * 10, 0, j * tileSize.y * 10), Quaternion.identity);
							break;
						case 1:
							Destroy (tiles [i, j].gameObject);
							tiles [i, j] = (GameObject)Instantiate (Cube4Wall, new Vector3 (i * tileSize.x * 10, 0, j * tileSize.y * 10), Quaternion.identity);
							break;
						case 2:
							Destroy (tiles [i, j].gameObject);
							tiles [i, j] = (GameObject)Instantiate (Cube3Wall, new Vector3 (i * tileSize.x * 10, 0, j * tileSize.y * 10), Quaternion.identity);
							break;
						case 3:
							Destroy (tiles [i, j].gameObject);
							tiles [i, j] = (GameObject)Instantiate (Cube2WallCorner, new Vector3 (i * tileSize.x * 10, 0, j * tileSize.y * 10), Quaternion.identity);
							break;
						}

						tiles [i, j].GetComponent<Transform> ().localScale = new Vector3 (tileSize.x, 0.1f, tileSize.y);
						tiles [i, j].transform.parent = GameObject.Find ("MapTiles").transform;
					}

					#endregion
				}
			}
		}

		for (int i = 1; i < floorSize.x-3; i++) {
			if (!mapTile [i, 1].GetVisited () && !mapTile [i, (int)(scaleSize - 2)].GetVisited ()) {
				if (i != 2 && i != scaleSize - 2) {

					int rand = Random.Range (0, 7);

					switch (rand) {
					case 0:
						ReplaceCell (i, 1, CubeLeftShortProps1, tileSize.x, tileSize.y, 270);
						break;
					case 1:
						ReplaceCell (i, 1, CubeLeftShortProps2, tileSize.x, tileSize.y, 270);
						break;
					case 2:
						ReplaceCell (i, 1, CubeLeftShortProps3, tileSize.x, tileSize.y, 270);
						break;
					case 3:
						ReplaceCell (i, 1, CubeLeftShortProps4, tileSize.x, tileSize.y, 270);
						break;
					case 4:
						ReplaceCell (i, 1, CubeLeftShortProps5, tileSize.x, tileSize.y, 270);
						break;
					case 5:
						ReplaceCell (i, 1, CubeLeftShortProps6, tileSize.x, tileSize.y, 270);
						break;
					case 6:
						ReplaceCell (i, 1, CubeLeftShortProps7, tileSize.x, tileSize.y, 270);
						break;
					}

					rand = Random.Range (0, 7);

					switch (rand) {
					case 0:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShortProps1, tileSize.x, tileSize.y, 90);
						break;
					case 1:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShortProps2, tileSize.x, tileSize.y, 90);
						break;
					case 2:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShortProps3, tileSize.x, tileSize.y, 90);
						break;
					case 3:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShortProps4, tileSize.x, tileSize.y, 90);
						break;
					case 4:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShortProps5, tileSize.x, tileSize.y, 90);
						break;
					case 5:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShortProps6, tileSize.x, tileSize.y, 90);
						break;
					case 6:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShortProps7, tileSize.x, tileSize.y, 90);
						break;
					}
				}
			}
		}

		for (int j = 1; j < floorSize.y-3; j++) {
			if (!mapTile [1, j].GetVisited () && !mapTile [(int)(scaleSize - 2),j].GetVisited ()) {
				if (j != 2 && j != scaleSize - 2) {

					int rand = Random.Range (0, 7);

					switch (rand) {
					case 0:
						ReplaceCell (1, j, CubeLeftShortProps1, tileSize.x, tileSize.y, 0);
						break;
					case 1:
						ReplaceCell (1, j, CubeLeftShortProps2, tileSize.x, tileSize.y, 0);
						break;
					case 2:
						ReplaceCell (1, j, CubeLeftShortProps3, tileSize.x, tileSize.y, 0);
						break;
					case 3:
						ReplaceCell (1, j, CubeLeftShortProps4, tileSize.x, tileSize.y, 0);
						break;
					case 4:
						ReplaceCell (1, j, CubeLeftShortProps5, tileSize.x, tileSize.y, 0);
						break;
					case 5:
						ReplaceCell (1, j, CubeLeftShortProps6, tileSize.x, tileSize.y, 0);
						break;
					case 6:
						ReplaceCell (1, j, CubeLeftShortProps7, tileSize.x, tileSize.y, 0);
						break;
					}

					rand = Random.Range (0, 7);

					switch (rand) {
					case 0:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShortProps1, tileSize.x, tileSize.y, 180);
						break;
					case 1:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShortProps1, tileSize.x, tileSize.y, 180);
						break;
					case 2:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShortProps1, tileSize.x, tileSize.y, 180);
						break;
					case 3:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShortProps1, tileSize.x, tileSize.y, 180);
						break;
					case 4:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShortProps1, tileSize.x, tileSize.y, 180);
						break;
					case 5:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShortProps1, tileSize.x, tileSize.y, 180);
						break;
					case 6:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShortProps1, tileSize.x, tileSize.y, 180);
						break;
					}
				}
			}
		}
			
		for (int i = 0; i < floorSize.x; i++) {
			for (int j = 0; j < floorSize.y; j++) {
				if (!mapTile [i, j].GetVisited ()) {

					if ((i == 1 || j == 1 || i == floorSize.y - 2 || j == floorSize.y - 2)) {

						if (i == 1 && j == 1) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 180);
						}
						if (i == 1 && j == floorSize.y - 2) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 270);
						}
						if (i == floorSize.y - 2 && j == 1) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 90);
						}
						if (i == floorSize.y - 2 && j == floorSize.y - 2) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 0);
						}
					}
				}
			}
		}

		if (!mapTile [(scaleSize/2)-1, (scaleSize/2)-1].GetVisited () && !mapTile [(scaleSize/2),(scaleSize/2)-1].GetVisited () &&
			!mapTile [(scaleSize/2)-1, (scaleSize/2)].GetVisited () && !mapTile [(scaleSize/2),(scaleSize/2)].GetVisited ()) {

			ReplaceCell ((int)(scaleSize/2)-1, (scaleSize/2)-1, CubeTEST, tileSize.x, tileSize.y, 0);
			ReplaceCell ((int)(scaleSize/2), (scaleSize/2)-1, CubeTEST, tileSize.x, tileSize.y, 0);
			ReplaceCell ((int)(scaleSize/2)-1, (scaleSize/2), CubeTEST, tileSize.x, tileSize.y, 0);
			ReplaceCell ((int)(scaleSize/2), (scaleSize/2), CubeTEST, tileSize.x, tileSize.y, 0);

		}
	}

	void ReplaceCell(int i, int j, GameObject instObj, float tileSizeX, float tileSizeY, int rotAngle){
		if(tiles[i,j])
			Destroy (tiles [i, j].gameObject);
		tiles [i, j] = (GameObject)Instantiate (instObj, new Vector3 (i * tileSizeX * 10, 0, j * tileSizeY * 10), Quaternion.identity);
		tiles [i, j].transform.Rotate (new Vector3(0,rotAngle,0));
		tiles [i, j].GetComponent<Transform> ().localScale = new Vector3 (tileSizeX, 0.1f, tileSizeY);
		tiles [i, j].transform.parent = GameObject.Find ("MapTiles").transform;
	}
}