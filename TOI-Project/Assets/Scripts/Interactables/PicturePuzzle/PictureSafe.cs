using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureSafe : MonoBehaviour
{
	public AudioClip impactSound;

	public void ImpactSound()
	{
		if (impactSound)
			AudioSource.PlayClipAtPoint(impactSound, transform.position);

	}
}
