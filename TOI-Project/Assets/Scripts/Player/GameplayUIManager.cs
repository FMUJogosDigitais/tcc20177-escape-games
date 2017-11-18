using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{

	//Todos as telas de gameplay
	[Header("Gameplay UI Screens")]
	[SerializeField]
	GameObject[] screens = { };

	#region Singleton
	//Início do Singleton
	public static GameplayUIManager Instance;
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
			originalFixedDeltaTime = Time.fixedDeltaTime;
			#endregion
		}
	}
	private void Start()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.PauseNotification += Pause;
			GameManager.Instance.ResumeNotification += Resume;
		}
	}

	private void OnDestroy()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.PauseNotification -= Pause;
			GameManager.Instance.ResumeNotification -= Resume;
		}
	}

	private void Pause()
	{
		ShowScreen("");
	}

	private void Resume()
	{
		ShowScreen("GameplayPanel");
	}

	public void ShowScreen(string name)
	{
		foreach (GameObject screen in screens)
		{
			screen.SetActive(screen.name == name);
		}
	}

	private void ReleaseDroppable()
	{
		Interactions.Instance.cannotDrop = false;
	}

	public void ReturnToGameplay()
	{
		ShowScreen("GameplayPanel");
		HoldGame(false);
		Invoke("ReleaseDroppable", .5f);
		Interactions.Instance.HideFeedback();
	}

	public void HoldGameplay()
	{
		Interactions.Instance.cannotDrop = true;
		HoldGame(true);
	}

	static float originalFixedDeltaTime;

	private void HoldGame(bool pauseStatus)
	{
		if (pauseStatus)
		{
			Time.timeScale = 0f;
			Time.fixedDeltaTime = 0f;
		}
		else
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = originalFixedDeltaTime;
		}
	}
}

