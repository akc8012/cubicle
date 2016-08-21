// Andrew
using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
	public float dist = 1;
	public float vert = 1;
	public Transform camStopA;
	public Transform camStopB;

	Transform player;
	float startY;
	//float startX;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		startY = player.position.y;
		//startX = transform.position.x;

		ForceSetCam();
	}

	void LateUpdate()
	{
		Vector3 target = player.position;
		Vector3 newPos;
		target.y = startY;

		newPos = target - Vector3.forward * dist;
		newPos += Vector3.up * vert;

		if (newPos.x != transform.position.x)
		{
			if (newPos.x < transform.position.x && newPos.x > camStopA.position.x ||
			newPos.x > transform.position.x && newPos.x < camStopB.position.x)
				transform.position = newPos;
			else
				transform.position = new Vector3(transform.position.x, newPos.y, newPos.z);
		}
		else
			transform.position = newPos;
	}

	public void ForceSetCam()
	{
		Vector3 target = player.position;
		Vector3 newPos;
		target.y = startY;

		newPos = target - Vector3.forward * dist;
		newPos += Vector3.up * vert;

		transform.position = newPos;
	}
}