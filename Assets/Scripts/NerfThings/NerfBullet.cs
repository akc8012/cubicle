//MICHAEL
using UnityEngine;
using System.Collections;

public class NerfBullet : MonoBehaviour
{
    public GameObject explosionParticle;

	void Start()
	{

	}

	void Update()
	{
		
	}

    void OnCollisionEnter(Collision col)
    {
        Instantiate(explosionParticle, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}