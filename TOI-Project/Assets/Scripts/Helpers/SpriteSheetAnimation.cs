using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetAnimation : MonoBehaviour
{

	Camera referenceCamera;

	public bool RotateTorwardsCamera = false;

	public enum Axis { up, down, left, right, forward, back };
	public bool reverseFace = false;
	public Axis axis = Axis.up;

	public int numberOfRows = 8;
	public int numberOfColumns = 8;

	private Vector2 tiling;
	private Vector2 offset;

	private int currentFrame = 0;
	private float counter = 0f;
	private int xIndex = 0;
	private int yIndex = 0;
	private float framePerSecond = 40;

	Material material;
	private const int loopStartFrame = 0;
	private int loopEndFrame;

	public Vector3 GetAxis(Axis refAxis)
	{
		switch (refAxis)
		{
			case Axis.down:
				return Vector3.down;
			case Axis.forward:
				return Vector3.forward;
			case Axis.back:
				return Vector3.back;
			case Axis.left:
				return Vector3.left;
			case Axis.right:
				return Vector3.right;
		}

		return Vector3.up;
	}

	// Use this for initialization
	void Start()
	{
		CalculateSizes();
		InitMaterial();
		InitCamera();
	}

	private void InitCamera()
	{
		if (!referenceCamera)
			referenceCamera = Camera.main;
	}

	private void InitMaterial()
	{
		material = GetComponent<MeshRenderer>().material;
		material.mainTextureScale = tiling;
	}

	private void CalculateSizes()
	{
		tiling.x = 1f / numberOfColumns;
		tiling.y = 1f / numberOfRows;
	}

	// Update is called once per frame
	void Update()
	{
		PlayAnimation();
		if (!RotateTorwardsCamera) return;
		UpdateRotation();
	}

	private void UpdateRotation()
	{
		Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
		Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);
		transform.LookAt(targetPos, targetOrientation);
	}

	private void PlayAnimation()
	{

		CalculateFrameIndex();

		xIndex = currentFrame % numberOfColumns;
		yIndex = currentFrame / numberOfColumns;

		offset = new Vector2(xIndex * tiling.x, ((numberOfRows - 1) - yIndex) * tiling.y);

		material.mainTextureOffset = offset;
	}

	private void CalculateFrameIndex()
	{
		counter += Time.deltaTime * framePerSecond;

		loopEndFrame = numberOfColumns * numberOfRows;

		if (counter >= 1f)
		{
			currentFrame += 1;
			counter = 0f;
		}

		if (currentFrame >= loopEndFrame)
		{
			currentFrame = loopStartFrame;
		}
	}
}
