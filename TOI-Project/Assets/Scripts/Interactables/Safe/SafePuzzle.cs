using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Badin;
using System;

public class SafePuzzle : MonoBehaviour
{
	public Transform safeDial;
	public readonly string Password = "535";
	public string TypedPassword = "000";
	public string currentNumber = "0";
	public AudioSource safeAudio;
	public AudioClip safeOpenAudio;
	public AudioClip safeDialAudio;
	public AudioClip safeButtonPressAudio;
	public GameObject diario;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void UpdateAnswer()
	{
		TypedPassword = TypedPassword.Substring(1, 2);
		TypedPassword += currentNumber;
		TypedPassword = TypedPassword.Substring(0, Mathf.Min(TypedPassword.Length, 3));

		if (TypedPassword == Password) AcertoMizeravi();
	}

	private void AcertoMizeravi()
	{
		diario.SetActive(true);
		safeDialAudio = null;
		safeButtonPressAudio = null;
		animator.SetBool("Open", true);
		if (safeOpenAudio) AudioSource.PlayClipAtPoint(safeOpenAudio, safeDial.position);
	}
}
