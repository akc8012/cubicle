// Andrew
using UnityEngine;
using System.Collections;

public class StealthFlop : MonoBehaviour
{
	public float dist = 0.15f;
	ZoomCam zoomCam;
	float startY;

	void Start()
	{
		zoomCam = GameObject.FindWithTag("MainCamera").GetComponent<ZoomCam>();
		startY = transform.position.y;
	}

	void Update()
	{
		Vector3 newPos;

		if (Input.GetButton("Stealth"))
		{
			newPos = new Vector3(transform.position.x, -dist, transform.position.z);
			zoomCam.ZoomOut();
		}
		else
		{
			newPos = new Vector3(transform.position.x, startY, transform.position.z);
			zoomCam.ZoomIn();
		}

		transform.position = newPos;
	}
}