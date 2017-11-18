using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorLever : InteractablesBehaviour
{
	public bool switchOn;
	public float leverOnAngle = -50f;
	public float leverOffAngle = 50f;
	public SwitchState state = SwitchState.Null;

	[Range(1f, 15f)] [SerializeField] float speed = 10f;
	public AudioClip switchSound;

	private Quaternion leverOnRotation = Quaternion.identity;
	private Quaternion leverOffRotation = Quaternion.identity;
	private bool isMoving = false;

	private void Start()
	{
		leverOnRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, leverOnAngle);
		leverOffRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, leverOffAngle);
	}

	public void Interaction()
	{
		if (isMoving) return;

		if (switchOn)
		{
			StartCoroutine(MoveLever(leverOffRotation));
		}
		else
		{
			StartCoroutine(MoveLever(leverOnRotation));
		}
	}

	private IEnumerator MoveLever(Quaternion leverRotation)
	{
		if (switchSound) AudioSource.PlayClipAtPoint(switchSound, transform.position);

		isMoving = true;

		while (Quaternion.Angle(transform.localRotation, leverRotation) > 0.5f)
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, leverRotation, speed * Time.deltaTime);
			yield return null;
		}
		switchOn = !switchOn;

		if (switchOn) state = SwitchState.Up;
		else state = SwitchState.Down;

		Generator.Instance.SetSwitch(this, state);

		isMoving = false;
		yield break;
	}
}
