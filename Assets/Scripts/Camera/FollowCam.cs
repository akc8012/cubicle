﻿// Andrew
using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
	public float dist = 1;
	public float vert = 1;

	Transform player;
	float startY;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		startY = player.position.y;
	}

	void LateUpdate()
	{
		Vector3 target = player.position;
		target.x = 0;
		target.y = startY;

		transform.position = target - Vector3.forward * dist;
		transform.position += Vector3.up * vert;
	}
}