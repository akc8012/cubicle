// Andrew
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookAtPlayer : MonoBehaviour
{
	public float rayY = 0.6f;

	//Transform player;
	List<Ray> whiskers = new List<Ray>();

	void Start()
	{
		//player = GameObject.FindWithTag("Player").transform;

	}

	void Update()
	{
		//transform.Rotate(new Vector3(0, 1, 0));

		Vector3 rayLoc = transform.position;
		rayLoc.y += rayY;

		CreateWhiskers(rayLoc);

		for (int i = 0; i < whiskers.Count; i++)
		{
			RaycastHit hit;

			if (Physics.Raycast(whiskers[i], out hit))
				Debug.DrawLine(whiskers[i].origin, hit.point);
		}
	}

	void CreateWhiskers(Vector3 rayLoc)
	{
		for (int i = 0; i < 10; i++)
		{
			Vector3 rayDir = transform.forward;
			rayDir = Quaternion.Euler(0, i*3, 0) * rayDir;
			Ray ray = new Ray(rayLoc, rayDir);
			whiskers.Add(ray);
		}
	}
}