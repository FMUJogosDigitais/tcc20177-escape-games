using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
	[System.Serializable]
	public struct PuzzlePicture
	{
		public PicturePlace picturePlace;
		public DropplableObject expectedPicture;
	}
	public PuzzlePicture[] puzzlePicture;

	public Rigidbody pictureVault;
	public Light safeLight;

	public void CheckAnswer()
	{
		for (int i = 0; i < puzzlePicture.Length; i++)
		{
			if (puzzlePicture[0].picturePlace.dropplableReceived == 
				puzzlePicture[0].expectedPicture && 
				puzzlePicture[1].picturePlace.dropplableReceived == 
				puzzlePicture[1].expectedPicture)
				AcertoMizeravi();
		}

	}

	private void AcertoMizeravi()
	{
		safeLight.intensity = 1f;
		LightsManager.Instance.Intensity = 0f;
		PlayerSanity.Instance.Scare(1f);
		pictureVault.isKinematic = false;
	}
}
