using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
	CanvasGroup group;
	float a = 0;
	//LevelManager levelManager;
	bool readyToR = false;
	public ProceduralMapGeneration mapGen;

	void Start()
	{
		group = GetComponent<CanvasGroup>();
		//levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}

	void Update()
	{
		if (readyToR && Input.GetButtonDown("Restart"))
		{
			StartCoroutine(FadeOutAndDie());
			GameObject.Find("LoseCanvas").GetComponent<Canvas>().enabled = false;
			readyToR = false;
		}
	}

	public void FIAD()
	{
		StartCoroutine(FadeInAndDie());
	}

	public void FIANL()
	{
		StartCoroutine(FadeInAndNewLevel());
	}

	IEnumerator FadeInAndDie()
	{
		while (group.alpha < 0.95f)
		{
			a += Time.deltaTime;
			group.alpha = a;
			yield return null;
		}
		group.alpha = 1;
		GameObject.FindWithTag("Player").GetComponent<Movement>().Reset();
		GameObject.FindWithTag("MainCamera").GetComponent<FollowCam>().ForceSetCam();
		GameObject.FindWithTag("Player").GetComponent<Movement>().SetMovement(true);
		GameObject.Find("LoseCanvas").GetComponent<Canvas>().enabled = true;
		readyToR = true;
	}

	IEnumerator FadeOutAndDie()
	{
		while (group.alpha > 0.05f)
		{
			a -= Time.deltaTime;
			group.alpha = a;
			yield return null;
		}
		group.alpha = 0;
	}

	IEnumerator FadeInAndNewLevel()
	{
		while (group.alpha < 0.95f)
		{
			a += Time.deltaTime;
			group.alpha = a;
			yield return null;
		}
		group.alpha = 1;
		GameObject player = GameObject.FindWithTag("Player");
		player.GetComponent<Movement>().Reset();
		player.GetComponent<Movement>().SetMovement(true);
		StartCoroutine(FadeOutAndNewLevel());
		//levelManager.NextLevel();
		mapGen.GenerateMap();
	}

	IEnumerator FadeOutAndNewLevel()
	{
		while (group.alpha > 0.05f)
		{
			a -= Time.deltaTime;
			group.alpha = a;
			yield return null;
		}
		group.alpha = 0;
	}
}