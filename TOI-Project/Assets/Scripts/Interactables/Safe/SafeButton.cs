using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeButton : InteractablesBehaviour
{
	public SafePuzzle safePuzzle;

	Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void Interaction()
	{
		animator.SetTrigger("Press");
		if (safePuzzle.safeButtonPressAudio)
		{
			safePuzzle.safeAudio.clip = safePuzzle.safeButtonPressAudio;
			if (safePuzzle.safeAudio)
			{
				if (!safePuzzle.safeAudio.isPlaying)
				{
					safePuzzle.safeAudio.Play();
				}
			}
		}
		safePuzzle.UpdateAnswer();
		//Debug.Log("Interact com " + this.name);
	}
}
