//MICHAEL
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public int floorCounter;

    public Text text;
	void Start()
	{
        floorCounter = 1;
        text.text = "Floor Count: " + floorCounter;
	}

	void Update()
	{
        text.text = "Floor Count:  " + floorCounter;
	}

    public void CountUp()
    {
        floorCounter++;
    }
}