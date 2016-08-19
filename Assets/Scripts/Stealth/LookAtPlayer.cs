// Andrew
using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour
{
	public float rayY = 0.6f;

	//Transform player;

	void Start()
	{
		//player = GameObject.FindWithTag("Player").transform;
	}

	void Update()
	{
		//transform.Rotate(new Vector3(0, 1, 0));

		Vector3 rayLoc = transform.position;
		rayLoc.y += rayY;

		Ray ray = new Ray(rayLoc, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
			Debug.DrawLine(ray.origin, hit.point);
	}
}