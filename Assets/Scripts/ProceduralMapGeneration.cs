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
	MapTile[] mapTileArray;

	GameObject[,] tiles;
	GameObject emptyTile, 
		Cube2Wall,  Cube2WallCornerTall,  Cube2WallCorner, 
		Cube3Wall,  Cube4Wall,  CubeTEST,
		CubeLeftShort, CubeLeftTall,

		CubeLeftShortProps1, CubeLeftShortProps2,
		CubeLeftShortProps3, CubeLeftShortProps4,
		CubeLeftShortProps5, CubeLeftShortProps6,
		CubeLeftShortProps7,

		CubeEmptyProps1, CubeEmptyProps2,
		CubeEmptyProps3, CubeEmptyProps4,
		CubeEmptyProps5, CubeEmptyProps6,

		Cube2WallCornerProps1, Cube2WallCornerProps2,
		Cube2WallCornerProps3, Cube2WallCornerProps4,

		CubeDoor;

	public GameObject enemyDude;
	public Transform targetPos;
	public Transform putPlayer;
	public Transform nextLevel;
	GameObject playerDude;
	GameObject map;
	int numberOfEnemies;

	void Start()
	{
		playerDude = GameObject.FindGameObjectWithTag("Player");
		map = GameObject.FindWithTag("Map");
		GenerateMap();
	}

	void Update()
	{
		
	}

	public void GenerateMap()
	{
		DeletePrior();
		Initialize();

		FindAssets();
		CreateBasicTileLayout();
		DefineStartEnd();
		FillGrid();
		PlaceEnemies();

		ScaleParent();
		PlacePlayer();
	}

	void DeletePrior()
	{
		Transform[] oldObjects = map.GetComponentsInChildren<Transform>();
		for (int i = 0; i < oldObjects.Length; i++)
		{
			if (oldObjects[i].tag != "Map" && oldObjects[i].tag != "PutPlayer" && oldObjects[i].tag != "NextLevel")
				Destroy(oldObjects[i].gameObject);
		}

		ResetParent();
	}

	void Initialize(){

		mapTile = new MapTile[scaleSize, scaleSize];
		tiles = new GameObject[scaleSize, scaleSize];
		floorSize = new Vector2(scaleSize,scaleSize);	// width, length - always square
		mapTileArray = new MapTile[200];

		numberOfEnemies = Random.Range (3, 9);
	}

	void PlaceEnemies(){

		int c = 0;

		for (int i = 0; i < scaleSize; i++) {
			for (int j = 0; j < scaleSize; j++) {
				if (mapTile [i, j].GetVisited ()) {
					
					mapTileArray [c] = mapTile [i, j];
					mapTileArray [c].SetPosition (new Vector3 (i, 0, j));
					c++;
				}
			}
		}

		for (int i = 0; i < scaleSize; i++) {
			for (int j = 0; j < scaleSize; j++) {
				if (mapTile [i, j].GetEmptyTileCheck ()) {

					mapTileArray [c] = mapTile [i, j];
					mapTileArray [c].SetPosition (new Vector3 (i, 0, j));
					c++;
				}
			}
		}

		for (int i = 0; i < numberOfEnemies; i++) {

			int rand = Random.Range (0, c);
			GameObject enemyInst = Instantiate (enemyDude, mapTileArray [rand].GetPosition(), Quaternion.identity) as GameObject;
			enemyInst.GetComponent<Transform> ().localPosition = new Vector3 (
				enemyInst.GetComponent<Transform> ().localPosition.x,
				0.35f,
				enemyInst.GetComponent<Transform> ().localPosition.z);
			enemyInst.GetComponent<Transform> ().localScale = new Vector3 (0.35f, 0.35f, 0.35f);
			enemyInst.transform.parent = map.transform;
		}
	}

	void FindAssets(){

		emptyTile 			= Resources.Load ("GenericModules/GenericTile") as GameObject;
		CubeTEST 			= Resources.Load ("GenericModules/CubeTEST") as GameObject;
		CubeLeftShort 		= Resources.Load ("GenericModules/CubeLeftShort") as GameObject;
		CubeLeftTall 		= Resources.Load ("GenericModules/CubeLeftTall") as GameObject;
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

		CubeEmptyProps1 	= Resources.Load ("_prefabs/CubeEmptyProps1") as GameObject;
		CubeEmptyProps2 	= Resources.Load ("_prefabs/CubeEmptyProps2") as GameObject;
		CubeEmptyProps3 	= Resources.Load ("_prefabs/CubeEmptyProps3") as GameObject;
		CubeEmptyProps4 	= Resources.Load ("_prefabs/CubeEmptyProps4") as GameObject;
		CubeEmptyProps5 	= Resources.Load ("_prefabs/CubeEmptyProps5") as GameObject;
		CubeEmptyProps6 	= Resources.Load ("_prefabs/CubeEmptyProps6") as GameObject;

		Cube2WallCornerProps1 	= Resources.Load ("_prefabs/Cube2WallCornerProps1") as GameObject;
		Cube2WallCornerProps2 	= Resources.Load ("_prefabs/Cube2WallCornerProps2") as GameObject;
		Cube2WallCornerProps3 	= Resources.Load ("_prefabs/Cube2WallCornerProps3") as GameObject;
		Cube2WallCornerProps4 	= Resources.Load ("_prefabs/Cube2WallCornerProps4") as GameObject;

		CubeDoor 			= Resources.Load ("GenericModules/CubeLeftTallDoor") as GameObject;
	}
		
	void CreateBasicTileLayout(){

		for (int i = 0; i < floorSize.x; i++)
			for (int j = 0; j < floorSize.y; j++) {

				ReplaceCell (i, j, emptyTile, tileSize.x, tileSize.y, 0);
				mapTile[i,j] = new MapTile (new Vector2(tiles[i,j].transform.position.x, tiles[i,j].transform.position.z), false);
				mapTile [i, j].SetEmptyTileCheck (true);
			}
	}

	void DefineStartEnd(){

		int randStart;
		Vector2 startSpot = new Vector2 (0, 0);
		Vector2 endSpot = new Vector2 (0, 0);

		randStart = (int)Random.Range (0+1, floorSize.y-1);

		startSpot = new Vector2(0, randStart);
		//ReplaceCell (0, randStart, CubeDoor, tileSize.x, tileSize.y, 0); 

		endSpot = new Vector2(floorSize.x -1, floorSize.y - randStart -1);
		ReplaceCell ((int)(floorSize.x -1), (int)(floorSize.y - randStart -1), CubeDoor, tileSize.x, tileSize.y, 180);
		nextLevel.position = new Vector3(endSpot.x+0.5f, nextLevel.position.y, endSpot.y);


		mapTile[(int)startSpot.x, (int)startSpot.y].SetVisited (true);
		mapTile[(int)startSpot.x, (int)startSpot.y].SetPosition (new Vector3(startSpot.x, 0, startSpot.y));

		putPlayer.position = new Vector3 (startSpot.x, 0, startSpot.y);
		putPlayer.localScale = new Vector3 (0.35f, 0.35f, 0.35f);

		mapTile[(int)endSpot.x, (int)endSpot.y].SetVisited (true);
		mapTile[(int)endSpot.x, (int)endSpot.y].SetPosition (new Vector3(endSpot.x, 0, endSpot.y));

		FindPath (startSpot, endSpot);
	}

	void FindPath(Vector2 start, Vector2 end){

		// choose how many turns the path will make
		int previousLength = 0;
		int cutPoint;

		// choose random lengths of each turn for number of turns
		// lengths have to be between (previousLength) and (maxLength/turnCount)
		int[] lengths = new int[2];
		int temp = Random.Range (1, scaleSize-1);
		cutPoint = temp;
	
		for (int i = 0; i < 2; i++) {
			
			lengths [i] = temp - previousLength;
			previousLength = lengths [i];
			temp = scaleSize;
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
				mapTile [i, j].SetEmptyTileCheck (true);
				if (!mapTile [i, j].GetVisited ()) {

					#region boundary walls
					if (i == 0 || j == 0 || i == floorSize.y - 1 || j == floorSize.y - 1 || i == 2 || j == 2 || i == 7 || j == 7) {
						
						//if (i == 0) {
						//	ReplaceCell (i, j, CubeLeftTall, tileSize.x, tileSize.y, 0);
						//}
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
						//if (i == 0 && j == 0) {
						//	ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 180);
						//}
						//if (i == 0 && j == floorSize.y - 1) {
						//	ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 270);
						//}
						if (i == floorSize.y - 1 && j == 0) {
							ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 90);
						}
						if (i == floorSize.y - 1 && j == floorSize.y - 1) {
							ReplaceCell (i, j, Cube2WallCornerTall, tileSize.x, tileSize.y, 0);
						}

					} else {

						int randCase = Random.Range (0, 100);
						int randRotation = Random.Range (0, 4);
						if(randRotation == 0) randRotation = 0;
						if(randRotation == 1) randRotation = 90;
						if(randRotation == 2) randRotation = 180;
						if(randRotation == 3) randRotation = 270;

						if(randCase >= 0 && randCase < 10)
							ReplaceCell (i, j, CubeEmptyProps1, tileSize.x, tileSize.y, randRotation);
						
						if(randCase >= 10 && randCase < 14)
							ReplaceCell (i, j, CubeEmptyProps2, tileSize.x, tileSize.y, randRotation);

						if(randCase >= 14 && randCase < 24)
							ReplaceCell (i, j, CubeEmptyProps3, tileSize.x, tileSize.y, randRotation);

						if(randCase >= 24 && randCase < 32)
							ReplaceCell (i, j, CubeEmptyProps4, tileSize.x, tileSize.y, randRotation);

						if(randCase >= 32 && randCase < 40)
							ReplaceCell (i, j, CubeEmptyProps5, tileSize.x, tileSize.y, randRotation);

						if(randCase >= 40 && randCase < 50)
							ReplaceCell (i, j, CubeEmptyProps6, tileSize.x, tileSize.y, randRotation);

						if(randCase >= 50 && randCase < 70)
							ReplaceCell (i, j, Cube3Wall, tileSize.x, tileSize.y, randRotation);

						if(randCase >= 70 && randCase < 90)
							ReplaceCell (i, j, Cube2Wall, tileSize.x, tileSize.y, randRotation);

						if(randCase >= 90 && randCase < 100)
							ReplaceCell (i, j, Cube4Wall, tileSize.x, tileSize.y, randRotation);

						tiles [i, j].GetComponent<Transform> ().localScale = new Vector3 (tileSize.x, 0.1f, tileSize.y);
						tiles [i, j].transform.parent = map.transform;
					}

					#endregion
				}
			}
		}

		for (int i = 1; i < floorSize.x-3; i++) {
			if (!mapTile [i, 1].GetVisited () && !mapTile [i, (int)(scaleSize - 2)].GetVisited ()) {
				if (i != 2 && i != scaleSize - 2) {

					int rand = Random.Range (0, 8);

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
					case 7:
						ReplaceCell (i, 1, CubeLeftShort, tileSize.x, tileSize.y, 270);
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
					case 7:
						ReplaceCell (i, (int)(floorSize.y - 2), CubeLeftShort, tileSize.x, tileSize.y, 90);
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
					case 7:
						ReplaceCell (1, j, CubeLeftShort, tileSize.x, tileSize.y, 0);
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
					case 7:
						ReplaceCell ((int)(floorSize.y - 2), j, CubeLeftShort, tileSize.x, tileSize.y, 180);
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
							int randCase = Random.Range (0, 100);

							if(randCase >= 0 && randCase < 10)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 180);
							if(randCase >= 10 && randCase < 20)
								ReplaceCell (i, j, Cube2WallCornerProps2, tileSize.x, tileSize.y, 180);
							if(randCase >= 20 && randCase < 30)
								ReplaceCell (i, j, Cube2WallCornerProps3, tileSize.x, tileSize.y, 180);
							if(randCase >= 30 && randCase < 40)
								ReplaceCell (i, j, Cube2WallCornerProps4, tileSize.x, tileSize.y, 180);
							if(randCase >= 40 && randCase < 100)
								ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 180);
						}
						if (i == 1 && j == floorSize.y - 2) {
							int randCase = Random.Range (0, 100);

							if(randCase >= 0 && randCase < 10)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 270);
							if(randCase >= 10 && randCase < 20)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 270);
							if(randCase >= 20 && randCase < 30)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 270);
							if(randCase >= 30 && randCase < 40)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 270);
							if(randCase >= 40 && randCase < 100)
								ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 270);
						}
						if (i == floorSize.y - 2 && j == 1) {
							int randCase = Random.Range (0, 100);

							if(randCase >= 0 && randCase < 10)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 90);
							if(randCase >= 10 && randCase < 20)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 90);
							if(randCase >= 20 && randCase < 30)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 90);
							if(randCase >= 30 && randCase < 40)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 90);
							if(randCase >= 40 && randCase < 100)
								ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 90);
						}
						if (i == floorSize.y - 2 && j == floorSize.y - 2) {
							int randCase = Random.Range (0, 100);

							if(randCase >= 0 && randCase < 10)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 0);
							if(randCase >= 10 && randCase < 20)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 0);
							if(randCase >= 20 && randCase < 30)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 0);
							if(randCase >= 30 && randCase < 40)
								ReplaceCell (i, j, Cube2WallCornerProps1, tileSize.x, tileSize.y, 0);
							if(randCase >= 40 && randCase < 100)
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
		if (tiles [i, j])
			Destroy (tiles [i, j].gameObject);
		tiles [i, j] = (GameObject)Instantiate (instObj, new Vector3 (i * tileSizeX * 10, 0, j * tileSizeY * 10), Quaternion.identity);
		tiles [i, j].transform.Rotate (new Vector3(0,rotAngle,0));
		tiles [i, j].GetComponent<Transform> ().localScale = new Vector3 (tileSizeX, 0.1f, tileSizeY);
		tiles [i, j].transform.parent = map.transform;
	}

	void ScaleParent()
	{
		map.transform.localScale = new Vector3(3, 3, 3);
		map.transform.rotation = Quaternion.Euler(0, -90, 0);
		map.transform.position = targetPos.position;
	}

	void ResetParent()
	{
		map.transform.localScale = Vector3.one;
		map.transform.rotation = Quaternion.identity;
		map.transform.position = Vector3.zero;
	}

	void PlacePlayer()
	{
		playerDude.transform.position = new Vector3(putPlayer.position.x, playerDude.transform.position.y, putPlayer.position.z);
		playerDude.GetComponent<Movement>().NextLevel();
		GameObject.FindWithTag("MainCamera").GetComponent<FollowCam>().ForceSetCam();
	}
}