using UnityEngine;
using System.Collections;

public class NerfBullet : MonoBehaviour
{
    //Grab the instance of the nerf gun
    private GameObject nerfGun;

    private Vector3 position;

	void Start()
	{
        nerfGun = GameObject.FindGameObjectWithTag("Gun");
        position = this.transform.position;
	}

	void Update()
	{
        position += Vector3.forward;
        this.transform.localPosition = position;
	}
}