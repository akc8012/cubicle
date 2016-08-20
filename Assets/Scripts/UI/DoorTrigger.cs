using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
	Transform player;
	ScreenFade screenFade;
	bool triggered = false;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		screenFade = GameObject.FindWithTag("Canvas").GetComponent<ScreenFade>();
	}

	void Update()
	{
		if (Vector3.Distance(player.position, transform.position) < 2 && !triggered)
		{
			screenFade.FIANL();
			GameObject.FindWithTag("Player").GetComponent<Movement>().SetMovement(false);
			triggered = true;
		}
		
		if (triggered && Vector3.Distance(player.position, transform.position) >= 2)
		{
			triggered = false;
		}
	}
}