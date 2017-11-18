using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInventory : MonoBehaviour
{

	[Serializable]
	public struct InventoryItems
	{
		public PickableObject pickableObject;
		public GameObject leftHandVisual;
		public GameObject rightHandVisual;
	}

	public InventoryItems[] inventory;

	public void GetItem(PickableObject item, PlayerHand hand)
	{
		ObjectInHand(item, hand);
		SwapObjectVisibility(item, hand, true);
	}

	public void DropItem(PickableObject item, PlayerHand hand)
	{
		RemoveObjectInHand(hand);
		SwapObjectVisibility(item, hand, false);
	}

	private void SwapObjectVisibility(PickableObject item, PlayerHand hand, bool visible)
	{

		foreach (InventoryItems i in inventory)
		{
			if (i.pickableObject == item)
			{
				if (!i.leftHandVisual || !i.rightHandVisual) return;

				if (hand == PlayerHand.Left)
				{
					GameObject objLeft = Interactions.Instance.carriedObjectLeft;
					ToggleVisualObject(objLeft, !visible);
					ToggleVisualObject(i.leftHandVisual, visible);
				}
				else if (hand == PlayerHand.Right)
				{
					GameObject objRight = Interactions.Instance.carriedObjectRight;
					ToggleVisualObject(objRight, !visible);
					ToggleVisualObject(i.rightHandVisual, visible);
				}
			}
		}
	}

	private void ObjectInHand(PickableObject item, PlayerHand hand)
	{

		DropplableObject dropItem;

		if (item.tag == "Alavanca")
			dropItem = DropplableObject.Lever;
		else if (item.tag == "Lamparina")
			dropItem = DropplableObject.Lamp;
		else if (item.tag == "Quadro_1")
			dropItem = DropplableObject.Picture_1;
		else if (item.tag == "Quadro_3")
			dropItem = DropplableObject.Picture_3;
		else
			dropItem = DropplableObject.GenericItem;


		if (hand == PlayerHand.Left) // Mão esquerda
			Interactions.Instance.objectInLeftHand = dropItem;
		else// Mão direita 
			Interactions.Instance.objectInRightHand = dropItem;
	}

	private void RemoveObjectInHand(PlayerHand hand)
	{
		if (hand == PlayerHand.Left)
			Interactions.Instance.objectInLeftHand = DropplableObject.None;
		else
			Interactions.Instance.objectInRightHand = DropplableObject.None;
	}

	public void ToggleVisualObject(GameObject gameObj, bool visible)
	{
		foreach (MeshRenderer meshRenderers in gameObj.GetComponentsInChildren<MeshRenderer>())
			meshRenderers.enabled = visible;

		MeshRenderer meshRenderer = gameObj.GetComponent<MeshRenderer>();

		if (meshRenderer) meshRenderer.enabled = visible;

	}

}
