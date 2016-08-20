using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour, ISelectHandler
{
    AudioSource sound;

	void Start()
	{
		sound = GetComponent<AudioSource>();
	}

	void Update()
	{
		
	}

    public void OnSelect(BaseEventData eventData)
    {
		sound.Play();
    }
}