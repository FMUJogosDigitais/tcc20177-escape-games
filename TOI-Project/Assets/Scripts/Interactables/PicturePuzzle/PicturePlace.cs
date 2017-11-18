using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicturePlace : InteractablesBehaviour
{
	public PicturePuzzle picturePuzzle;
	public GameObject pictureObject;
	public Transform pictureTransform;
	public DropplableObject dropplableReceived;
	public AudioClip pictureDropSound;


	public new void DropPicture_1()
	{
		Debug.Log("DropPicture_1 recebido");
		dropplableReceived = DropplableObject.Picture_1;
		CatchPicture();
	}

	public new void DropPicture_3()
	{
		Debug.Log("DropPicture_3 recebido");
		dropplableReceived = DropplableObject.Picture_3;
		CatchPicture();
	}

	private void CatchPicture()
	{
		AudioSource.PlayClipAtPoint(pictureDropSound, transform.position);
		pictureObject = Interactions.Instance.carriedObjectRight;
		Interactions.Instance.DropObject(PlayerHand.Right);
		Rigidbody rb;
		rb = pictureObject.GetComponent<Rigidbody>();
		rb.isKinematic = true;
		rb.transform.SetParent(this.transform);
		rb.transform.position = pictureTransform.position;
		rb.transform.rotation = pictureTransform.rotation;

		picturePuzzle.CheckAnswer();
	}

	public void Interaction()
	{
		Debug.Log("Interact com " + this.name);
	}
}
