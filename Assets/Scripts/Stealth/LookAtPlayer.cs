// Andrew
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookAtPlayer : MonoBehaviour
{
	public float rayY = 0.6f;
	public float turningSpeed = 12;
	public ComeAtTarget comeAtTarget;

	Transform player;
	Collider col;
	List<Ray> whiskers = new List<Ray>();
	Vector3 postPos;
	bool found = false;
	float possibleTime;
	float lookAtTime = 0.5f;    // timer must reach
	float comeAtTimer;

	enum States { Searching, GoTowards, GoBack };
	States state;

	void Start()
	{
		postPos = transform.position;
		player = GameObject.FindWithTag("Player").transform;
		col = GetComponent<Collider>();
		state = States.Searching;
		possibleTime = -1;
		comeAtTimer = -1;
	}

	void Update()
	{
		if (state == States.Searching)
			Search();
		if (state == States.GoTowards)
		{
			if (comeAtTimer != -1 && Time.timeSinceLevelLoad - comeAtTimer < 3)
			{
				if (comeAtTarget.ComeAt())
					state = States.GoBack;
			}
			else
			{
				state = States.GoBack;
				col.enabled = false;
			}
		}
		if (state == States.GoBack)
		{
			if (comeAtTarget.ToPost(postPos))
			{
				state = States.Searching;
				col.enabled = true;
			}
		}
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
			possibleTime = -1;
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
			yield return null;
		}

		state = States.GoTowards;
		comeAtTimer = Time.timeSinceLevelLoad;
	}
}