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
	public Animator anim;
	bool found = false;
	float possibleTime;
	float lookAtTime = 0.5f;    // timer must reach
	float comeAtTimer;
	Vector3 goToTarget;
	bool goToTargetSet = false;
	float startY;
	AudioSource audioSource;

	enum States { Searching, GoTowards, ForceGoTowards, GoBack };
	States state;

	void Start()
	{
		postPos = transform.position;
		player = GameObject.FindWithTag("Player").transform;
		col = GetComponent<Collider>();
		state = States.Searching;
		possibleTime = -1;
		comeAtTimer = -1;
		startY = transform.position.y;
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (state == States.Searching)
			Search();
		if (state == States.GoTowards || state == States.ForceGoTowards)
		{
			if (state == States.ForceGoTowards || comeAtTimer != -1 && Time.timeSinceLevelLoad - comeAtTimer < 3)
			{
				Vector3 target = goToTargetSet ? goToTarget : player.position;

				if (comeAtTarget.ComeAt(target))
				{
					state = States.GoBack;
					goToTargetSet = false;

					if (target == player.position && GameObject.FindWithTag("Player").GetComponent<Movement>().CanGet())
					{
						GameObject.FindWithTag("Canvas").GetComponent<ScreenFade>().FIAD();
						GameObject.FindWithTag("Player").GetComponent<Movement>().SetMovement(false);
						return;
					}
				}
			}
			else
			{
				state = States.GoBack;
				col.enabled = false;
				goToTargetSet = false;
			}
		}
		if (state == States.GoBack)
		{
			if (comeAtTarget.ToPost(postPos))
			{
				state = States.Searching;
				col.enabled = true;
				anim.SetFloat("Walk", 0);
			}
		}

		transform.position = new Vector3(transform.position.x, startY, transform.position.z);

		if (state == States.GoTowards || state == States.ForceGoTowards || state == States.GoBack)
			anim.SetFloat("Walk", 1);
		else
			anim.SetFloat("Walk", 0);
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
				if (hit.collider.gameObject.GetComponent<StealthFlop>().GetVisible())
				{
					found = true;

					if (possibleTime == -1)
						possibleTime = Time.timeSinceLevelLoad;

					break;
				}
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
		audioSource.Play();
	}

	public void SetGoToTarget(Vector3 target)
	{
		if (Vector3.Distance(target, transform.position) < 10)
		{
			goToTarget = target;
			goToTargetSet = true;
			state = States.GoTowards;
			comeAtTimer = Time.timeSinceLevelLoad;
			StopAllCoroutines();
		}
	}
}