using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeNumber : InteractablesBehaviour
{
	public SafePuzzle safePuzzle;
	public int thisNumber;

	void Start()
	{
		defaultMaterial = null;
		highlightedMaterial = null;
		meshRenderer = null;
	}

	public new void HighlightObject()
	{
		safePuzzle.safeDial.localRotation = this.transform.localRotation;
		safePuzzle.currentNumber = thisNumber.ToString();
		//if (safePuzzle.safeAudio)
		//{
		//	safePuzzle.safeAudio.clip = safePuzzle.safeDialAudio;
		//	if (!safePuzzle.safeAudio.isPlaying)
		//	{
		//		safePuzzle.safeAudio.Play();
		//	}
		//}
	}

	public new void DeselectObject() { }

	public void Interaction() { }

}
