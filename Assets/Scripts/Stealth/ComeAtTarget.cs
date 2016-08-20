using UnityEngine;
using System.Collections;

public class ComeAtTarget : MonoBehaviour
{
	public float goToSpeed = 0.5f;

	Transform player;
	Vector3 target;
	CharacterController controller;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		controller = GetComponent<CharacterController>();
	}

	public bool ComeAt()
	{
		Vector3 dirVector = player.position - transform.position;
		dirVector.y = 0;
		Quaternion rot = Quaternion.LookRotation(dirVector);

		transform.rotation = rot;
		controller.Move(transform.forward * goToSpeed);

		if (Vector3.Distance(player.position, transform.position) < 1.5f)
		{
			return true;
		}

		return false;
	}

	public bool ToPost(Vector3 target)
	{
		Vector3 dirVector = target - transform.position;
		dirVector.y = 0;
		Quaternion rot = Quaternion.LookRotation(dirVector);

		transform.rotation = rot;
		transform.position += transform.forward * goToSpeed;

		if (Vector3.Distance(target, transform.position) < 1)
		{
			return true;
		}

		return false;
	}
}