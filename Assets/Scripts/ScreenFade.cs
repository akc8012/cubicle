using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
	CanvasGroup group;
	float a = 0;

	void Start()
	{
		group = GetComponent<CanvasGroup>();
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
		GameObject.FindWithTag("Player").GetComponent<Movement>().SetMovement(true);
		StartCoroutine(FadeOutAndDie());
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
		GameObject.FindWithTag("Player").GetComponent<Movement>().Reset();
		GameObject.FindWithTag("Player").GetComponent<Movement>().SetMovement(true);
		StartCoroutine(FadeOutAndNewLevel());
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