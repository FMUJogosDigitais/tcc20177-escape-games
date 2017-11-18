using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
/// <summary>
/// Retorna o objeto ao local original após cair fora do cenário
/// </summary>
public class PickableObject : InteractablesBehaviour
{
	public bool needBothHands = false;

	public bool ResetPositionWhenOutOfSight = true;

	public AudioClip impactSound;

	private Vector3 initialObjectPosition;
	private Quaternion initialObjectRotation;

	private Rigidbody Rigidbody;

	//public float floorHeight, centerObjectHeight, minimumObjectHeight;
	//private Ray rayDown, inverseRayDown;

	private void Start()
	{
		Rigidbody = GetComponent<Rigidbody>();
		StartCoroutine(LateStart());
	}

	IEnumerator LateStart()
	{
		yield return new WaitForSeconds(1f);
		initialObjectPosition = transform.position;
		initialObjectRotation = transform.rotation;
		yield break;
	}

	public void ResetObject()
	{
		transform.position = initialObjectPosition;
		transform.rotation = initialObjectRotation;
		if (Rigidbody)
		{
			Rigidbody.velocity = Vector3.zero;
			Rigidbody.angularVelocity = Vector3.zero;
			Rigidbody.isKinematic = true;
		}
	}

	private void OnBecameInvisible()
	{
		if (ResetPositionWhenOutOfSight)
			ResetObject();
	}

	bool impact = false;
	public new void ImpactSound()
	{
		if (!impact)
		{
			impact = true;
			if (impactSound) AudioSource.PlayClipAtPoint(impactSound, transform.position);
			Invoke("NullImpact", .5f);
		}
	}

	void NullImpact()
	{
		impact = false;
	}

	/// <summary>
	/// Método chamado pelo Interactions do Player
	/// </summary>
	public new void GetFeedback()
	{
		base.GetFeedback();

		if (needBothHands)
		{
			if (!Interactions.Instance.IsBothHandsFree() && !Interactions.Instance.IsCarryingWithBothHands)
			{
				ShowFeedback(ItemDescriptionKeyNoHand);
			}
			return;
		}
	}


	//private void FixedUpdate()
	//{
	//	UpdateRaycasts();
	//	UpdateObjectPostion();
	//}

	//private void UpdateObjectPostion()
	//{
	//	Vector3 newPosition = transform.position;

	//	newPosition.y = Mathf.Clamp(newPosition.y, minimumObjectHeight, Mathf.Infinity);

	//	transform.position = newPosition;
	//}

	//private void UpdateRaycasts()
	//{
	//	rayDown = new Ray(transform.position, transform.InverseTransformDirection(-transform.up));

	//	RaycastHit hitDown, inverseHitDown;

	//	if (Physics.Raycast(rayDown, out hitDown))
	//	{
	//		floorHeight = hitDown.point.y;
	//		inverseRayDown = new Ray(hitDown.point, transform.InverseTransformDirection(transform.up));

	//		if (Physics.Raycast(inverseRayDown, out inverseHitDown))
	//		{
	//			centerObjectHeight = Vector3.Distance(inverseHitDown.point, transform.position);
	//		}
	//	}

	//	minimumObjectHeight = floorHeight + centerObjectHeight;
	//}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.blue;
	//	Gizmos.DrawRay(rayDown);

	//}
}
