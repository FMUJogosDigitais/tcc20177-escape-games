using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActionDoor : MonoBehaviour
{
	public Door door;
	public AudioClip slamDoor;

	public TriggerActionPiano trigPiano;

	private bool isTriggered = false;

	public void LookupAction()
	{
		if (isTriggered) return;
		StartCoroutine(DoorClosingCutscene());
	}

	private IEnumerator DoorClosingCutscene()
	{
		isTriggered = true;

		PlayerSanity.Instance.Scare(20);
		FPSController.Instance.SlowDown();
		yield return new WaitForSeconds(1f);
		FPSController.Instance.RestoreOriginalSpeed();
		door.speed = 8f;
		door.rightDoor.speed = 8f;
		door.IsLocked = false;
		AudioClip closeSound = door.doorCloseSound;
		door.doorCloseSound = slamDoor;
		door.rightDoor.doorCloseSound = null;
		door.rightDoor.IsLocked = false;
		door.Interaction();
		door.rightDoor.Interaction();
		if (!Keychain.Instance.haveKey[3])
			door.IsLocked = true;
		yield return new WaitForSeconds(1f);
		door.speed = 1.25f;
		door.rightDoor.speed = 1.25f;
		door.doorCloseSound = closeSound;
		door.rightDoor.doorCloseSound = closeSound;
		trigPiano.StartMusic();
		Destroy(this.gameObject);
	}
}
