//MICHAEL
using UnityEngine;
using System.Collections;

public class NerfGun : MonoBehaviour
{

    public GameObject nerfBullet;   //Get the prefab for the nerf bullets
    private GameObject bulletSpawnPoint; //Get the spawn point which is child of this object

    private int shotsPerSecond;     //How many nerfs can the gun fire per second
    private float range;            //How far the gun can shoot a nerf in METERS
    public float bulletForce;       //The force the projectile will have

    public Vector3 position;         //The gun position
    public Quaternion rotation;      //The gun rotation

    //Upgrades or "Power Ups" for the gun
    enum States
    {
        UPGRADE1 = 0,
        UPGRADE2 = 1,
        UPGRADE3 = 2
    }
    States state;  //Instance of States


    void Awake()
    {
        bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
        GunPowerUp(0);
    }

	void Update()
	{
        position = this.transform.position;
        rotation = this.transform.rotation;

        //Small FSM for the different states of the gun for upgrades.
        switch(state)
        {
            case (States)0:
                ShotsPerSecond = 5;
                Range = 10f;
                break;
            case (States)1:
                ShotsPerSecond = 7;
                Range = 20f;
                break;
            case (States)2:
                ShotsPerSecond = 10;
                Range = 30f;
                break;
        }
        //-------------------------------------------------------//

        if (Input.GetKeyDown(KeyCode.Space))
            Fire();
	}

    //Switches the gun between the different upgrades states
    public void GunPowerUp(int x)
    {
        switch(x)
        {
            case 0:
                state = States.UPGRADE1;
                break;
            case 1:
                state = States.UPGRADE2;
                break;
            case 2:
                state = States.UPGRADE3;
                break;
        }
    }
    //-----------------------------------------------------------//

    //Fires the gun when trigger is pulled
    public void Fire()
    {
        //Shortcut for the bullet spawn point's location
        Vector3 bulletPlace = bulletSpawnPoint.transform.position;
        Quaternion bulletRot = bulletSpawnPoint.transform.rotation;

        //Grab the bullet and instantiate it.  Fix the rotation.
        GameObject theBullet;
        theBullet = Instantiate(nerfBullet, bulletPlace, bulletRot) as GameObject;
        //theBullet.transform.Rotate(Vector3.left * 90);

        //Set the rigid body of the nerf bullet and then apply a forward force to it
        Rigidbody bulletRb;
        bulletRb = theBullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * bulletForce);
        
    }

    #region Variable Helpers
    //Set and Get the shotsPerSecond variable
    public int ShotsPerSecond
    {
        set { shotsPerSecond = value; }
        get { return shotsPerSecond; }
    }
    //Set and Get the range variable
    public float Range
    {
        set { range = value; }
        get { return range; }
    }

    #endregion
}