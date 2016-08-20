﻿// TRISTAN
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
	public GameObject emptyTile, Cube2Wall, Cube2WallCorner, Cube3Wall, Cube4Wall, CubeLeft;

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

		emptyTile 		= Resources.Load ("_prefabs/GenericTile") as GameObject;
		CubeLeft 		= Resources.Load ("_prefabs/CubeLeft") as GameObject;
		Cube2Wall 		= Resources.Load ("_prefabs/Cube2Wall") as GameObject;
		Cube2WallCorner = Resources.Load ("_prefabs/Cube2WallCorner") as GameObject;
		Cube3Wall		= Resources.Load ("_prefabs/Cube3Wall") as GameObject;
		Cube4Wall 		= Resources.Load ("_prefabs/Cube4Wall") as GameObject;
	}
		
	void CreateBasicTileLayout(){

		for (int i = 0; i < floorSize.x; i++)
			for (int j = 0; j < floorSize.y; j++) {

				tiles[i,j] = (GameObject)Instantiate (emptyTile, new Vector3 (i * tileSize.x * 10, 0, j * tileSize.y * 10), Quaternion.identity);
				tiles[i,j].GetComponent<Transform> ().localScale = new Vector3 (tileSize.x, 1, tileSize.y);
				tiles[i,j].transform.parent = GameObject.Find ("MapTiles").transform;
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

		int colorShift = (int)start.x;
		for (int h = 0; h < lengths[0]; h++) {
			
			tiles[(int)colorShift + h + 1, (int)start.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
			mapTile [(int)colorShift + h + 1, (int)start.y].SetVisited (true);
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
					
					if (i != 0 && j != 0 && i != 9 && j != 9 && i != 2 && j != 2 && i != 7 && j != 7) {
						Debug.Log (i + " : " + j);
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

						tiles [i, j].GetComponent<Transform> ().localScale = new Vector3 (tileSize.x, 1, tileSize.y);
						tiles [i, j].transform.parent = GameObject.Find ("MapTiles").transform;

					} else {
						

						if (i == 0) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 0);
						}
						if (j == 0) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 270);
						}
						if (j == 9) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 90);
						}
						if (i == 9) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 180);
						}
						if (i == 0 && i == 2) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 0);
						}
						if (j == 0 && j == 2) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 270);
						}
						if (j == 9 && j == 7) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 90);
						}
						if (i == 9 && i == 7) {
							ReplaceCell (i, j, CubeLeft, tileSize.x, tileSize.y, 180);
						}
						if (i == 0 && j == 0) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 270);
						}
						if (i == 0 && j == 9) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 0);
						}
						if (i == 9 && j == 0) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 180);
						}
						if (i == 9 && j == 9) {
							ReplaceCell (i, j, Cube2WallCorner, tileSize.x, tileSize.y, 90);
						}
					}
				}
			}
		}
	}

	void ReplaceCell(int i, int j, GameObject instObj, float tileSizeX, float tileSizeY, int rotAngle){
		Destroy (tiles [i, j].gameObject);
		tiles [i, j] = (GameObject)Instantiate (instObj, new Vector3 (i * tileSizeX * 10, 0, j * tileSizeY * 10), Quaternion.identity);
		tiles [i, j].transform.Rotate (new Vector3(0,rotAngle,0));
		tiles [i, j].GetComponent<Transform> ().localScale = new Vector3 (tileSizeX, 1, tileSizeY);
		tiles [i, j].transform.parent = GameObject.Find ("MapTiles").transform;
	}
}