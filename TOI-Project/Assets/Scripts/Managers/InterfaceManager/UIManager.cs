using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
/// <summary>
/// O UI Manager é resposável por controlar qual tela será exibida
/// assim como atualizar a HUD. 
/// </summary>
public class UIManager : MonoBehaviour
{

	WaitForSeconds wait = new WaitForSeconds(1f);

	//Todos as telas de menus
	[Header("UI Screens")]
	[SerializeField]
	GameObject[] screens = { };

	public const int NUMBER_OF_PAGES = 3;

	[Header("Page Buttons")]
	[SerializeField] GameObject[] pageButtons = new GameObject[NUMBER_OF_PAGES];


	/// <summary>
	/// Start é chamado no frame quando o script é habilitado antes de qualquer método Update pela primeira vez.
	/// </summary>
	void Start()
	{

		//NotificationCenter.AddObserver(this, "Pause");
		//NotificationCenter.AddObserver(this, "Resume");
		GameManager.Instance.PauseNotification += Pause;
		GameManager.Instance.ResumeNotification += Resume;

	}

	/// <summary>
	/// Exibe a tela de um nome específico e oculta do todo o resto
	/// </summary>
	/// <param name="name">Nome da tela a ser exibida</param>
	public void ShowScreen(string name)
	{
		// Loop por todas as telas no array
		foreach (GameObject screen in screens)
		{
			// Ativa a tela com o nome correspondente, e desativa qualquer tela que não tiver o mesmo nome
			screen.SetActive(screen.name == name);
		}
	}

	public void ShowScreenWithDelay(string name)
	{
		StartCoroutine(ShowScreenDelayed(name));
	}

	IEnumerator ShowScreenDelayed(string name)
	{
		yield return wait;
		ShowScreen(name);
	}

	public static void OnLanguageChanged()
	{
		foreach (StaticTextManagerTMPro staticText in FindObjectsOfType<StaticTextManagerTMPro>())
		{
			staticText.OnLanguageChanged();
		}
	}

	private void Pause()
	{
		ShowScreen("PauseMenuPanel");
	}

	private void Resume()
	{
		if (GameManager.Instance.currentState == GameManager.GameStates.MainGame)
		{
			ShowScreen("");
		}
	}

	internal void CheckForPages()
	{
		for (int i = 0; i < NUMBER_OF_PAGES; i++)
		{
			pageButtons[i].SetActive(PlayerPrefsManager.GetPageUnlocked(i));
		}
	}
}
