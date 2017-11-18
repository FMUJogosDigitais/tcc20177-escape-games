using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : InteractablesBehaviour {

	public PianoKeyboard piano;
	public int index;
	
	public void Interaction()
	{
		piano.PlayNote(index);
	}
}
