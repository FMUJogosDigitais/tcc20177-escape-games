using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTween;

[RequireComponent(typeof(BoxCollider))]
public class TriggerActionSound : MonoBehaviour {

    [SerializeField] private AudioSource sounds;
	public bool isTriggered = false;
	public TriggerActionDoor triggerActionDoor;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    
    }

    private void OnTriggerEnter(Collider other)
    {
		if (isTriggered) return;

        if (other.gameObject.tag == "Player")
        {
			if (triggerActionDoor) triggerActionDoor.LookupAction();
			SimpleTweener.AddTween(() => sounds.volume, vol => sounds.volume =  vol, 1f, 2.0f).Delay(.5f);
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (isTriggered) return;

		if (other.gameObject.tag == "Player")
        {
            SimpleTweener.AddTween(() => sounds.volume, vol => sounds.volume = vol, 0f, 2.0f).Delay(.5f);
			isTriggered = true;
		}
    }

}

