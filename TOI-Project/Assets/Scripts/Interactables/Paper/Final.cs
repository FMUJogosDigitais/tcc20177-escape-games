using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : InteractablesBehaviour
{

	public void Interaction()
	{
		GameplayUIManager.Instance.ShowScreen("CreditsPanel");
	}
}
