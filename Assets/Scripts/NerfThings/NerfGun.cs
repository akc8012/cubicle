//MICHAEL
using UnityEngine;
using System.Collections;

public class NerfGun : MonoBehaviour
{

    public GameObject nerfBullet;   //Get the prefab for the nerf bullets
    private GameObject bulletSpawnPoint; //Get the spawn point which is child of this object

    private float bulletForce;       //The force the projectile will have

    private bool hasFired;    //Flag for if the gun has fired
    private bool reloading;       //Flag for if the gun is reloading

    private float reloadTimer;     //The timer dedicated to reloading

    public Vector3 position;         //The gun position
    public Quaternion rotation;      //The gun rotation

    //Upgrades or "Power Ups" for the gun
    enum States
    {
        UPGRADE1 = 0,   //Default
        UPGRADE2 = 1,
        UPGRADE3 = 2
    }
    States state;  //Instance of States


    void Awake()
    {
        //Find the bullet spawn point
        bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
        //Set the Gun's state to the default
        GunPowerUp(1);

        HasFired = false;                   //Set HasFired to false at the beginning of the game
        ReloadTimer = 3f;                    //Set the reload timer to 3 seconds
        Reloading = false;                  //Set Reloading to false at the beginning of the game
    }


	void Update()
	{
        position = this.transform.position;
        rotation = this.transform.rotation;

        //Small FSM for the different states of the gun for upgrades.
        switch(state)
        {
            case (States)0:
                BulletForce = 500f;
                break;
            case (States)1:
                BulletForce = 1000f;
                break;
            case (States)2:
                BulletForce = 1500f;
                break;
        }
        //-------------------------------------------------------//

        //If the gun has not been fired
        if (!hasFired)
        {
           
            ReloadTimer = 3f;                //Set the reloading timer to 3 seconds every frame the HasFired is false
            if (Input.GetKeyDown(KeyCode.T))
            {
                Fire();
            }
        }

        //If the gun has been fired
        if(HasFired)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Reloading = true;
            }
        }
        if(Reloading)
        {
            Reload();
        }

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
        bulletRb.AddForce(transform.forward * BulletForce);

        HasFired = true;
        
    }

    //Reloads the nerf gun when given the proper input
    public void Reload()
    {
        ReloadTimer -= Time.deltaTime;      //Count the reload timer down
        if (ReloadTimer <= 0)                //If the reload timer is less than or equal to 0, switch the flag for hasFired back to off
        {
            HasFired = false;           //Set the gun to a fire-able state
            Reloading = false;          //Gun is no longer reloading
        }
    }


    #region Variable Helper Functions
    public bool HasFired
    {
        set { hasFired = value; }
        get { return hasFired; }
    }
    public bool Reloading
    {
        set { reloading = value; }
        get { return reloading; }
    }
    public float BulletForce
    {
        set { bulletForce = value; }
        get { return bulletForce; }
    }
    public float ReloadTimer
    {
        set { reloadTimer = value; }
        get { return reloadTimer; }
    }
    #endregion

}