//MICHAEL
using UnityEngine;
using System.Collections;

public class NerfGun : MonoBehaviour
{

    public GameObject nerfBullet;   //Get the prefab for the nerf bullets

    private int shotsPerSecond;     //How many nerfs can the gun fire per second
    private float range;            //How far the gun can shoot a nerf in METERS

    public Vector3 position;         //The gun position
    public float distanceFromMuzzle; //So we can change the spawn location of the bullet on the fly

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
        GunPowerUp(0);
    }

    void Start()
    {
        Fire();
    }

	void Update()
	{
        position = this.transform.position;

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
        Instantiate(nerfBullet, new Vector3(position.x, position.y, (transform.localPosition.z + distanceFromMuzzle)), Quaternion.identity);
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