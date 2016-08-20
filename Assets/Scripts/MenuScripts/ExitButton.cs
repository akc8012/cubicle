using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour, ISelectHandler
{
    AudioSource audio;

	void Start()
	{
        audio = GetComponent<AudioSource>();
	}

	void Update()
	{
		
	}

    public void OnSelect(BaseEventData eventData)
    {
        audio.Play();
    }
}