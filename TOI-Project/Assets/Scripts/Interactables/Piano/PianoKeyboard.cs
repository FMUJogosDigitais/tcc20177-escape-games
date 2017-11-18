using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyboard : MonoBehaviour
{
	public PianoKey[] pianoKeys;
	public AudioClip[] PianoNotes;

	// Use this for initialization
	void Start()
	{
		InitKeysIndex();
	}

	private void InitKeysIndex()
	{
		for (int i = 0; i < pianoKeys.Length; i++)
		{
			pianoKeys[i].index = i;
		}
	}

	public void PlayNote(int index)
	{

		AudioSource.PlayClipAtPoint(PianoNotes[index], transform.position, 2f);

	}
}
