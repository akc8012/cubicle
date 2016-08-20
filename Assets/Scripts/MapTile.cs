using UnityEngine;
using System.Collections;

public class MapTile
{
	// Type of tile
	GameObject tileType;
	Vector2 position;

	bool visited = false;

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
		
}