//MICHAEL
using UnityEngine;
using System.Collections;

public class NerfBullet : MonoBehaviour
{
    public GameObject explosionParticle;
	GameObject[] enemies;

	void Start()
	{
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	void Update()
	{
		
	}

    void OnCollisionEnter(Collision col)
    {
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].GetComponent<LookAtPlayer>().SetGoToTarget(col.gameObject.transform.position);
		}

		Instantiate(explosionParticle, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}