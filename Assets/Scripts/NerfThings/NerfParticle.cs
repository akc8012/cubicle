//MICHAEL
using UnityEngine;
using System.Collections;

public class NerfParticle : MonoBehaviour
{
    public float lifeTimer;

	void Start()
	{
        lifeTimer = .5f;
	}

	void Update()
	{
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
            Destroy(gameObject);
    }
}