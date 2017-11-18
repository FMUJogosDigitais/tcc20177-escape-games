using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : InteractablesBehaviour {

	public PicturePuzzle picturePuzzle;

	public void Interaction()
	{
		//Debug.Log("check quadros");
		//picturePuzzle.CheckAnswer();
	}

	public new void ImpactSound() { }
}
