using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerActionDespensa : MonoBehaviour
{
	public bool isTriggeredEnter = false;
	public bool isTriggeredLights = false;
	public string MessageFeedback;
	public AudioClip diabretes;
	public AudioClip desligandoALuz;
	private WaitForSeconds wait0_5 = new WaitForSeconds(.5f);
	public Door door;
	public AudioClip slamDoor;

	private void Start()
	{
		GetComponent<BoxCollider>().isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isTriggeredEnter) return;
		if (other.gameObject.tag == "Player")
		{
			isTriggeredEnter = true;
			//PlayerSanity.Instance.Scare(20);
			door.CloseDoorImmediately();
			AudioSource.PlayClipAtPoint(slamDoor, door.transform.position);
			door.IsLocked = true;
		}
	}

	public void StartDespensaCutscene()
	{
		StartCoroutine(DespensaCutscene());
	}

	private IEnumerator DespensaCutscene()
	{
		if (isTriggeredLights) yield break;
		door.speed = 1.25f;
		isTriggeredLights = true;
		PlayerSanity.Instance.Scare(20);
		PlayerSanity.Instance.noLightnings = true;
		do
		{
			if (diabretes != null) AudioSource.PlayClipAtPoint(diabretes, transform.position);
			for (int i = 0; i < 3; i++)
			{
				yield return wait0_5;
				LightsManager.Instance.Intensity = 0f;
				yield return wait0_5;
				LightsManager.Instance.Intensity = 1f;
				yield return wait0_5;
				LightsManager.Instance.Intensity = .2f;
			}
		} while (!Keychain.Instance.haveKey[1]);

		PlayerSanity.Instance.noLightnings = false;

		door.IsLocked = false;
		door.OpenDoorImmediately();
		PlayerSanity.Instance.Scare(2);
		yield return wait0_5;
		if (desligandoALuz) AudioSource.PlayClipAtPoint(desligandoALuz, transform.position);
		LightsManager.Instance.Intensity = 0f;

		Destroy(this);
		yield break;
	}
}
