using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[DisallowMultipleComponent]
/// <summary>
/// Classe que gerencia o jogo e os menus do jogo
/// </summary>
public class GameManager : MonoBehaviour
{
	private float gameClock;
	//SoundManager soundMan;
	[HideInInspector] public UIManager uiManager;
	public GameStates currentState = GameStates.LogoScreen;
	[Header("UI Elements")]
	public GameObject pressStart;
	public TextMeshProUGUI pressStartText;
	public GameObject hintPanel;
	[HideInInspector] public SceneLoader sceneLoader;
	[HideInInspector] public bool showSkipBox = false;
	[SerializeField] private float blinkSeconds = 3f;
	[SerializeField] private SceneField logoScene;
	[SerializeField] private SceneField startScreenScene;
	[SerializeField] private SceneField startMenuScene;
	[SerializeField] private SceneField introCutscene;
	[SerializeField] private SceneField mainGameScene;
	[Range(5f, 60f)] [SerializeField] private float timeout = 15f;
	private float time;
	private bool textEnabled = true;
	private WaitForSeconds wait = new WaitForSeconds(1);
	//private bool isGameOver = false;
	static float originalFixedDeltaTime;

	/// <summary>
	/// Adicionando Observers (Substituição do NotificationCenter)
	/// </summary>
	public delegate void OnGamePause();
	public event OnGamePause PauseNotification;

	public delegate void OnGameResume();
	public event OnGameResume ResumeNotification;

	//public delegate void OnStartNewGame();
	//public event OnStartNewGame InitNotification;

	public delegate void OnCutsceneSkip();
	public event OnCutsceneSkip CutsceneSkip;

	public static bool isPaused;

	public bool IsPaused
	{
		get { return isPaused; }
		set { isPaused = value; }
	}

	//public delegate void OnGameOver();
	//public OnGameOver GameOverNotification;


	public string GameSessionTime
	{
		get { return Badin.Extensions.FloatToTime(gameClock, "#0:00:00.00"); }
	}

	public int GameSessionSeconds
	{
		get { return Mathf.RoundToInt(gameClock); }
	}


	#region Singleton
	//Inicio do Singleton
	private static GameManager instance = null;
	public static GameManager Instance // Esse script possuí uma referência estática a si mesmo para que os outros scripts possam acessar sem precisar de uma referência para tal.
	{
		get { return instance; }
	}

	private void Awake()
	{
		// Essa é uma medida comum para manipular uma classe com uma referência a si mesma.
		// Se a variável da instância existir, mas não for este objeto, destrua este objeto.
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			// Caso contrário, este objeto será asignado para a instância.
			instance = this;
			// DontDestroyOnLoad(gameObject);
		}//Fim do Singleton
		#endregion
		#region Awake
		// Demais scripts Awake

		sceneLoader = GetComponent<SceneLoader>();
		originalFixedDeltaTime = Time.fixedDeltaTime;


		//soundMan = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
		if (!sceneLoader) sceneLoader = GetComponent<SceneLoader>();
		if (!uiManager) uiManager = GetComponent<UIManager>();

		pressStartText.enabled = false;
		if (hintPanel == null)
			hintPanel = GameObject.Find("HintPanel");

		//NotificationCenter.AddObserver(this, "GameOver"); //Método antigo com NotificationCenter
		//GameOverNotification += GameOver;
		#endregion
	}

	public enum GameStates
	{
		LogoScreen,
		Startup,
		StartScreen,
		StartMenu,
		CutsceneIntro,
		MainGame
	}

	private void Start()
	{
		time = Time.time;
		timeout = time + timeout;
		sceneLoader.faderCanvasGroup.alpha = 1f;

		OnLanguageChanged();
		StartGameSessionTimer();

		switch (currentState)
		{
			case GameStates.LogoScreen:
				StartCoroutine(Goto_Logo());
				break;
			case GameStates.Startup:
				StartCoroutine(Goto_Startup());
				break;
			case GameStates.StartScreen:
				Goto_StartScreen();
				break;
			case GameStates.StartMenu:
				Goto_StartMenu();
				break;
			case GameStates.CutsceneIntro:
				Goto_CutsceneIntro();
				break;
			case GameStates.MainGame:
				Goto_MainGame();
				break;
			default:
				break;
		}

	}

	private void StartGameSessionTimer()
	{
		gameClock = Time.time;
		gameClock -= gameClock;
	}

	private void Update()
	{
		UpdateGameSessionTimer();

		switch (Instance.currentState)
		{
			case GameStates.StartScreen:
				InputStartScreen();
				break;
			case GameStates.CutsceneIntro:
				InputCutscene();
				break;
			case GameStates.MainGame:
				InputInGame();
				break;
			default:
				break;
		}
	}

	private void UpdateGameSessionTimer()
	{
		gameClock += Time.deltaTime;
	}

	private void OnApplicationQuit()
	{

	}


	#region Inputs
	private void InputInGame()
	{
		//if (isGameOver) return;
		//TODO: Ações do player in game

		if (Input.GetButtonDown("UIMenu"))
		{
			PauseGame();
		}
	}


	private void InputStartScreen()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Goto_StartMenu();
		}
	}

	private void InputCutscene()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			CutsceneSkip();
		}
	}

	#endregion


	#region Goto

	private IEnumerator Goto_Logo()
	{
		yield return StartCoroutine(sceneLoader.LoadSceneAndSetActive(logoScene));
		StartCoroutine(Badin.Extensions.FadeCanvas(sceneLoader.faderCanvasGroup, 0f));
		yield return new WaitForSeconds(5f);
		StartCoroutine(Goto_Startup());
		Badin.Extensions.fadeDuration = 0.5f;
	}

	private IEnumerator Goto_Startup()
	{
		sceneLoader.FadeAndLoadScene(startScreenScene);
		StartCoroutine(Badin.Extensions.FadeCanvas(sceneLoader.faderCanvasGroup, 0f));
		yield return wait;
		StartCoroutine(Goto_StartScreen());
		Badin.Extensions.fadeDuration = 0.5f;
	}

	private IEnumerator Goto_StartScreen()
	{
		Instance.currentState = GameStates.StartScreen;
		//   bool isKeyboard = true;
		while (time < timeout)
		{
			pressStartText.enabled = textEnabled;
			if (textEnabled)
			{
				//if (isKeyboard) // Caso o input do jogador for do teclado
				//{
				pressStartText.spriteAsset = ControlIDManager.Instance.keyboardSpriteAsset;
				pressStartText.text = "Press enter";
				//    isKeyboard = false;
				//}
				//else // Caso o input do jogador for do controle
				//{
				//    pressStartText.spriteAsset = ControlIDManager.Instance.genericControllerSpriteAsset;
				//    pressStartText.text = Replace.GetControllerIcons(Language.Get("HINT_LABEL_PRESS_START"));
				//    isKeyboard = true;
				//}
			}
			textEnabled = !textEnabled;
			time = Time.time;
			yield return new WaitForSeconds(blinkSeconds);
		}
		if (Instance.currentState == GameStates.StartScreen)
		{
			Debug.Log("Saiu para cutscene");
			// TODO: Sair para a tela de demonstração ou cutscene
		}
		else
		{
			Debug.Log("Não saiu para cutscene");
		}


	}

	public void ReturnToMainMenu()
	{
		PauseGame(false);
		Goto_StartMenu();
		if (GameplayUIManager.Instance)
		{
			GameplayUIManager.Instance.ReturnToGameplay();
		}
		PauseGame(false);
		currentState = GameStates.StartMenu;
	}

	public void Goto_StartMenu()
	{
		if (currentState != GameStates.StartMenu)
			sceneLoader.FadeAndLoadScene(startMenuScene);
		Instance.currentState = GameStates.StartMenu;
		pressStart.SetActive(false);
		uiManager.ShowScreenWithDelay("StartMenu");
	}

	public void Goto_CutsceneIntro()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (currentState != GameStates.CutsceneIntro)
			sceneLoader.FadeAndLoadScene(introCutscene);
		Instance.currentState = GameStates.CutsceneIntro;
		uiManager.ShowScreen("");
	}

	public void Goto_MainGame()
	{
		if (currentState != GameStates.MainGame)
			sceneLoader.FadeAndLoadScene(mainGameScene);
		Instance.currentState = GameStates.MainGame;
		uiManager.ShowScreenWithDelay("Gameplay UI");
		sceneLoader.persistentCamera.SetActive(false);
		//Init();
	}

	public void Init()
	{
		//    InitNotification();
	}

	#endregion

	/// <summary>
	/// Recebe a chapada de Pausa através de um boolean
	/// </summary>
	/// <param name="pauseStatus"></param>
	public void PauseGame(bool pauseStatus)
	{
		if (pauseStatus)
		{
			Time.timeScale = 0f;
			Time.fixedDeltaTime = 0f;
			isPaused = true;
			//    NotificationCenter.PostNotification(null, "Pause");
			PauseNotification();
		}
		else if (!pauseStatus)
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = originalFixedDeltaTime;
			isPaused = false;
			//    NotificationCenter.PostNotification(null, "Resume");
			ResumeNotification();

		}
	}
	public void PauseGame()
	{
		isPaused = !isPaused;
		hintPanel.SetActive(IsPaused);
		PauseGame(isPaused);
	}

	/// <summary>
	/// Envia a todos os gameobjects quando a aplicação pausa 
	/// Como por exemplo, ao minimizar a janela ou sair com o botão "Home" do smartphone
	/// </summary>
	/// <param name="pauseStatus"></param>
	public void OnApplicationPause(bool pauseStatus)
	{
		// Apenas pausar ao sair mas não retomar ao voltar
		if (pauseStatus && currentState == GameStates.MainGame)
		{
			PauseGame(true);
		}
	}


	public void OnLanguageChanged()
	{
		UIManager.OnLanguageChanged();
	}

	public void MenuOpen_Options()
	{
		uiManager.ShowScreen("OptionsPanel");
	}


	public void MenuClose_Options()
	{
		switch (currentState)
		{
			case GameStates.StartMenu:
				uiManager.ShowScreen("StartMenu");
				break;
			case GameStates.MainGame:
				uiManager.ShowScreen("PauseMenuPanel");
				break;
			default:
				break;
		}
	}

	public void MenuOpen_Daybook()
	{
		uiManager.ShowScreen("DaybookPanel");
		uiManager.CheckForPages();
	}
}
