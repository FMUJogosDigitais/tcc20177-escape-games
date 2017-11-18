using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesBehaviour : MonoBehaviour
{
	public Material defaultMaterial;
	public Material highlightedMaterial;
	public string ItemDescriptionKey = "";
	public string ItemDescriptionKeyNoHand = "";
	public bool isHintKey = false;

	public MeshRenderer meshRenderer;

	/// <summary>
	/// Método chamado pelo Interactions do Player
	/// </summary>
	public void GetFeedback()
	{
		if (Interactions.Instance.IsCurrentHandFree())
		{
			ShowFeedback(ItemDescriptionKey);
		}
		else
		{
			ShowFeedback(ItemDescriptionKeyNoHand);
		}

	}

	public void ShowFeedback(string message)
	{
		Interactions.Instance.DisplayFeedback(message);
	}

	private void Start()
	{
		if (!meshRenderer)
			meshRenderer = GetComponent<MeshRenderer>();
		if (!meshRenderer) Debug.LogError("MeshRenderer não encontrado em " + this.name);
		DeselectObject();
	}


	public void HighlightObject()
	{
		meshRenderer.material = highlightedMaterial;
	}

	public void DeselectObject()
	{
		meshRenderer.material = defaultMaterial;
	}


	public void ImpactSound() { }

	public void DropPicture_1() { }

	public void DropPicture_3() { }
}