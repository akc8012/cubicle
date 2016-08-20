using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public GameObject level1;
	public GameObject level2;
	public GameObject level3;

	int level = 0;

	void Start()
	{

	}

	void Update()
	{
		
	}

	public void NextLevel()
	{
		level++;

		if (level == 1)
		{
			level1.SetActive(false);
			level2.SetActive(true);
		}
		if (level == 2)
		{
			level2.SetActive(false);
			level3.SetActive(true);
		}
	}
}