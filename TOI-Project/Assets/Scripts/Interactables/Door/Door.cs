using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Door : InteractablesBehaviour
{
	public float doorOpenAngle = 90.0f;
	public float doorClosedAngle = 0.0f;
	[Range(1f, 15f)] public float speed = 10f;
	public AudioClip doorOpenSound;
	public AudioClip doorCloseSound;
	public AudioClip doorLockedSound;
	public AudioClip useKeySound;

	public Sprite lockedIcon;
	public Sprite unlockedIcon;

	internal Quaternion doorOpen = Quaternion.identity;
	internal Quaternion doorClosed = Quaternion.identity;

	public bool doorIsOpen = false;
	private bool doorIsMoving = false;
	private float waitBeforeOpenDoor = 0f;

	public Door rightDoor;
	[HideInInspector] public Key doorKey;

	[SerializeField] private bool m_isLocked = false;

	public bool IsLocked
	{
		get
		{
			return m_isLocked;
		}
		set
		{
			m_isLocked = value;
			if (rightDoor) rightDoor.IsLocked = m_isLocked;
		}
	}

	private bool showFeedback = false;

	private void Start()
	{
		if (gameObject.isStatic)
		{
			Debug.LogWarning("A porta foi marcada como static e por isso o script Door foi removido.");
			Destroy(this);
		}
		doorOpen = Quaternion.Euler(transform.localEulerAngles.x, doorOpenAngle, transform.localEulerAngles.z);
		doorClosed = Quaternion.Euler(transform.localEulerAngles.x, doorClosedAngle, transform.localEulerAngles.z);
	}

	/// <summary>
	/// Método chamado pelo Interations do Player
	/// </summary>
	public void Interaction()
	{
		if (doorIsMoving) return;

		if (IsLocked)
		{
			if (doorLockedSound) AudioSource.PlayClipAtPoint(doorLockedSound, transform.position);
			showFeedback = true;
			Invoke("HideFeedback", 3f);
			return;
		}

		if (!doorIsOpen) //A porta está aberta?
		{
			if (doorKey) //Possuí uma chave?
			{
				if (rightDoor && !rightDoor.doorKey) //Possuí uma portaDireita sem chave?
				{
					doorKey = null; //Remove a chave
					goto MoveDoor; //Move a porta
				}
				else if (rightDoor && rightDoor.doorKey) //Possuí uma portaDireita com chave?
				{
					rightDoor.doorKey = null; //Retira a chave da portaDireita
				}
				Keychain.Instance.UseKey(doorKey); //Usa a chave correspondente (remove ela do inventário)
				if (useKeySound) AudioSource.PlayClipAtPoint(useKeySound, transform.position, 2f); //Som de usar a chave
				doorKey = null; //Remove a chave
				waitBeforeOpenDoor = 1f; // Espera "usar a chave".
			}
			else
				waitBeforeOpenDoor = 0f;
			MoveDoor:
			StartCoroutine(MoveDoor(doorOpen));
		}
		else
		{
			StartCoroutine(MoveDoor(doorClosed));
		}
	}

	public void CloseDoorImmediately()
	{
		StopCoroutine("MoveDoor");
		transform.localRotation = doorClosed;
	}

	public void OpenDoorImmediately()
	{
		StopCoroutine("MoveDoor");
		transform.localRotation = doorOpen;
	}

	internal IEnumerator MoveDoor(Quaternion doorTargetPosition)
	{
		if (!doorIsMoving)
		{
			yield return new WaitForSeconds(waitBeforeOpenDoor); //Cria um efeito de espera para destrancar com a chave

			if (!doorIsOpen)
			{
				if (doorOpenSound != null) AudioSource.PlayClipAtPoint(doorOpenSound, transform.position);
			}
			else
			{
				if (doorCloseSound != null) AudioSource.PlayClipAtPoint(doorCloseSound, transform.position);
			}
		}
		doorIsMoving = true;

		while (Quaternion.Angle(transform.localRotation, doorTargetPosition) > 0.5f)
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, doorTargetPosition, speed * Time.deltaTime);
			yield return null;
		}
		doorIsOpen = !doorIsOpen;
		doorIsMoving = false;
		yield return null;
	}


	public new void GetFeedback()
	{
		if (!showFeedback)
		{
			if (!IsLocked && doorKey)
			{
				Interactions.Instance.DisplayFeedback(unlockedIcon);
			}
		}
		else if (IsLocked && doorKey)
		{
			Interactions.Instance.DisplayFeedback(lockedIcon);
		}

		//base.GetFeedback();


	}

	private void HideFeedback()
	{
		showFeedback = false;
	}

}
