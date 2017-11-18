using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSwitch : DroppablesBehaviour
{
	public GameObject lever;
	private GeneratorLever generatorLever;
	private MeshRenderer leverMeshRenderer;

	private void Start()
	{
		generatorLever = lever.GetComponent<GeneratorLever>();
		leverMeshRenderer = lever.GetComponent<MeshRenderer>();

		generatorLever.enabled = false;
		leverMeshRenderer.enabled = false;
	}

	public void DropObject()
	{
		if (generatorLever.enabled) return;

		EnableSwitchLever();
	}

	public void Interact()
	{
		if (generatorLever.enabled)
			generatorLever.Interaction();
	}

	public void EnableSwitchLever()
	{
		generatorLever.enabled = true;
		leverMeshRenderer.enabled = true;
	}

}
