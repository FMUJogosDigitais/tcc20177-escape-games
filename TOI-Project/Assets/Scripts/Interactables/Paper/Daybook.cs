using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daybook : MonoBehaviour
{
	[System.Serializable]
	public struct Pages
	{
		public Paper paper;
		public GameObject paperUIname;
		public GameObject paperObj;
	}
	public Pages[] pages;

	#region Singleton
	//Início do Singleton
	public static Daybook Instance;
	private void Awake()
	{
		{
			if (Instance == null)
				Instance = this;

			else if (Instance != this)
				Destroy(this);
			//Fim do Singleton
			#endregion
			#region Awake
			// Scripts do Awake
			GetComponent<GameplayUIManager>();
			#endregion
		}
	}

	/// <summary>
	/// Adiciona a página no menu
	/// </summary>
	/// <param name="paper"></param>
	public void AddPage(Paper paper)
	{
		Interactions.Instance.lastHighlighted = null;
		for (int i = 0; i < pages.Length; i++)
		{
			if (pages[i].paper == paper)
			{
				GameplayUIManager.Instance.ShowScreen(pages[i].paperUIname.name);
				GameplayUIManager.Instance.HoldGameplay();
				if (i < UIManager.NUMBER_OF_PAGES)
				{
					PlayerPrefsManager.SetPageUnlocked(i, true);
					pages[i].paperObj.SetActive(false);
				}
			}
		}
	}
}
