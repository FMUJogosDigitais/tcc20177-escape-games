using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : InteractablesBehaviour
{
	public AudioClip paperSound;

	public void Interaction()
	{
		if (paperSound) AudioSource.PlayClipAtPoint(paperSound, transform.position, 2f);
		Daybook.Instance.AddPage(this);
	}
}
