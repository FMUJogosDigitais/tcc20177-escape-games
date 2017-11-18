using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHit : MonoBehaviour
{
	private void OnCollisionStay(Collision collision)
	{
		if (collision.rigidbody)
			collision.gameObject.SendMessageUpwards("ImpactSound");
	}
}
