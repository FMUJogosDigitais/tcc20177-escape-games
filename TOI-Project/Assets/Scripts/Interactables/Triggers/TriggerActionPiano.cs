using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerActionPiano : MonoBehaviour
{
	public bool isTriggeredEnter = false;
	public AudioClip pianoClip;
	public AudioClip pianoMusic;
	public GameObject pianoSalaEstar;
	private Animator pianoAnim;
	private AudioSource pianoAudio;

	private void Start()
	{
		GetComponent<BoxCollider>().isTrigger = true;
		if (!pianoSalaEstar)
			Debug.LogError("PianoSalaEstar não referenciado");
		else
		{
			pianoAnim = pianoSalaEstar.GetComponent<Animator>();
			pianoAudio = pianoSalaEstar.GetComponent<AudioSource>();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isTriggeredEnter) return;
		if (other.gameObject.tag == "Player")
		{
			isTriggeredEnter = true;
			PlayerSanity.Instance.Scare(20);
			pianoAudio.clip = pianoClip;
			pianoAudio.loop = false;
			pianoAudio.Play();
			pianoAnim.SetBool("ClosePiano", true);
			Invoke("StopAudio", 5f);
		}
	}

	public void StartMusic()
	{
		pianoAudio.clip = pianoMusic;
		pianoAudio.Play();
	}

	public void StopAudio()
	{
		pianoAudio.Stop();
	}

}
