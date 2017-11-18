using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCredits : MonoBehaviour
{
	public RectTransform txtCredits;
	public float scrollSpeed = 50;
	private float screenHeight;
	private float bottomPosition;
	private float topPosition;
	private float posY;
	public bool canExit = false;

	// Use this for initialization
	void Start()
	{
		screenHeight = Screen.currentResolution.height;
		topPosition = screenHeight / 2;
		posY = -screenHeight / 2;
		txtCredits.position = new Vector3(txtCredits.position.x, posY);
		StartCoroutine(ScrollCredits());
		FPSController.CanMove = false;
		PlayerSanity.Instance.isScared = false;
	}

	private IEnumerator ScrollCredits()
	{
		while (posY < topPosition)
		{
			posY += scrollSpeed * Time.deltaTime;
			txtCredits.position = new Vector3(txtCredits.position.x, posY);
			yield return null;
		}
		yield return new WaitForSeconds(5f);
		if (GameManager.Instance) GameManager.Instance.ReturnToMainMenu();

	}
}
