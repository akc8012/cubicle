// Andrew
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookAtPlayer : MonoBehaviour
{
	public float rayY = 0.6f;
	public float turningSpeed = 12;

	Transform player;
	List<Ray> whiskers = new List<Ray>();
	bool found = false;
	float possibleTime = -1;
	float lookAtTime = 0.5f;	// timer must reach

	enum States { Searching, GoTowards };
	States state;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		state = States.Searching;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
			state = States.Searching;

		if (state == States.Searching)
			Search();
		if (state == States.GoTowards)
			GoForth();
	}

	void Search()
	{
		found = false;

		Vector3 rayLoc = transform.position;
		rayLoc.y += rayY;

		CreateWhiskers(rayLoc);

		for (int i = 0; i < whiskers.Count; i++)
		{
			RaycastHit hit;

			if (Physics.Raycast(whiskers[i], out hit) && hit.collider.gameObject.tag == "Player")
			{
				found = true;

				if (possibleTime == -1)
					possibleTime = Time.timeSinceLevelLoad;

				break;
			}
		}

		if (!found)
		{
			possibleTime = -1;
			transform.Rotate(new Vector3(0, 1, 0));
		}

		if (possibleTime != -1 && Time.timeSinceLevelLoad - possibleTime > lookAtTime)
		{
			state = States.GoTowards;
			possibleTime = -1;
			print("found");
			StartCoroutine(QuickLookAt());
		}
	}

	void CreateWhiskers(Vector3 rayLoc)
	{
		whiskers.Clear();

		for (int i = 0; i < 10; i++)
		{
			Vector3 rayDir = transform.forward;
			rayDir = Quaternion.Euler(0, i*3, 0) * rayDir;
			Ray ray = new Ray(rayLoc, rayDir);
			whiskers.Add(ray);
		}

		for (int i = 0; i < 10; i++)
		{
			Vector3 rayDir = transform.forward;
			rayDir = Quaternion.Euler(0, i*-3, 0) * rayDir;
			Ray ray = new Ray(rayLoc, rayDir);
			whiskers.Add(ray);
		}
	}

	IEnumerator QuickLookAt()
	{
		Vector3 dirVector = player.position - transform.position;
		dirVector.y = 0;
		Quaternion rot = Quaternion.LookRotation(dirVector);

		while (Quaternion.Angle(transform.rotation, rot) > 3)
		{
			dirVector = player.position - transform.position;
			dirVector.y = 0;
			rot = Quaternion.LookRotation(dirVector);

			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turningSpeed);
			print("turning");
			yield return null;
		}
	}

	void GoForth()
	{
		//state = States.Searching;
	}
}