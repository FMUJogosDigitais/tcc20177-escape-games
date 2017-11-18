using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerActionHall : MonoBehaviour
{
	public TriggerActionDoor triggerActionDoor;

	private void Start()
	{
		GetComponent<BoxCollider>().isTrigger = true;

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (triggerActionDoor) triggerActionDoor.LookupAction();
			Destroy(this.gameObject);
		}
	}

}
