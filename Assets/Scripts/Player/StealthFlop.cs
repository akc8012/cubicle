// Andrew
using UnityEngine;
using System.Collections;

public class StealthFlop : MonoBehaviour
{
	public float dist = 0.15f;
	ZoomCam zoomCam;
	bool visible = true;

	void Start()
	{
		zoomCam = GameObject.FindWithTag("MainCamera").GetComponent<ZoomCam>();
	}

	void Update()
	{
		if (Input.GetButton("Stealth"))
		{
			zoomCam.ZoomOut();
			visible = false;
		}
		else
		{
			zoomCam.ZoomIn();
			visible = true;
		}
	}

	public bool GetVisible() { return visible; }
}