using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
	public int whichOne;

	Transform player;
	NerfGun nerfGun;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		nerfGun = GameObject.FindWithTag("Gun").GetComponent<NerfGun>();
	}

	void Update()
	{
		if (Vector3.Distance(transform.position, player.position) < 1)
		{
			if (whichOne == 0)
				nerfGun.powerUp = true;
			if (whichOne == 1)
				nerfGun.powerUp2 = true;

			gameObject.SetActive(false);
		}
	}
}