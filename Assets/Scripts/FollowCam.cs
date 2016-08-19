// Andrew
using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
	public float dist = 1;
	public float vert = 1

	Transform player;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
	}

	void LateUpdate()
	{
		Vector3 target = player.position;
		target.x = 0;

		transform.position = target - Vector3.forward * dist;
		transform.position += Vector3.up * vert;
	}
}