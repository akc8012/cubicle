﻿// Andrew
using UnityEngine;
using System.Collections;

public class ZoomCam : MonoBehaviour
{
	public float startZoom = 60;
	public float outZoom = 75;
	public float zoomSpeed = 20;

	Camera cam;
	GameObject[] derp;

	void Start()
	{
		cam = GetComponent<Camera>();
		derp = GameObject.FindGameObjectsWithTag("Player");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P)) print(derp.Length);
	}

	public void ZoomOut()
	{
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, outZoom, zoomSpeed * Time.deltaTime);
	}

	public void ZoomIn()
	{
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, startZoom, zoomSpeed * Time.deltaTime);
	}
}