using UnityEngine;
using System.Collections;

public class MapTile
{
	// Type of tile
	GameObject tileType;
	Vector3 position;

	bool visited = false;
	bool emptyTileCheck = false;

	public MapTile(
		Vector2 _pos,
		bool _visited)
	{
		position = _pos;
		visited = _visited;
	}

	public void SetVisited(bool newValue){
		visited = newValue;
	}

	public bool GetVisited(){
		return visited;
	}

	public void SetEmptyTileCheck(bool newValue){
		emptyTileCheck = newValue;
	}

	public bool GetEmptyTileCheck(){
		return emptyTileCheck;
	}
		
	public void SetPosition(Vector3 newValue){
		position = newValue;
	}

	public Vector3 GetPosition(){
		return position;
	}
}