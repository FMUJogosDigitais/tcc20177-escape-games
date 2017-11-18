using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Badin;

[DisallowMultipleComponent]
/// <summary>
/// Classe que controla as funções do game no escopo geral
/// </summary>
public class SceneLoader : MonoBehaviour
{
    //public event Action BeforeSceneUnload;
    //public event Action AfterSceneLoad;

    public static int LevelToLoad;

    public CanvasGroup faderCanvasGroup;


    public GameObject persistentCamera;

    /// <summary>
    /// Fecha o jogo ou para de rodar a cena quando rodado no editor da Unity
    /// </summary>
    public void QuitGame()
    {
        //Se estamos rodando no jogo compilado
#if UNITY_STANDALONE
        //Fecha o jogo
        Application.Quit();
#endif
        //Se estamos rodando no editor
#if UNITY_EDITOR
        //Para a cena
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    /// <summary>
    ///  Vai para o menu principal, indicado no número do build settings ou fecha o jogo se já estiver no menu principal
    /// </summary>
    /// <param name="mainMenuSceneNumber">Número da cena no build settings</param>
    public void MainMenuOrQuitGame(int mainMenuSceneNumber)
    {

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == mainMenuSceneNumber)
        {
            Debug.Log("menuSceneNum: " + mainMenuSceneNumber + "buildIndex: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            //Se estamos rodando no jogo compilado
#if UNITY_STANDALONE
            //Fecha o jogo
            Application.Quit();
#endif
            //Se estamos rodando no editor
#if UNITY_EDITOR
            //Para a cena
            UnityEditor.EditorApplication.isPlaying = false;

#endif
        }
        else
        {
            LoadScene(mainMenuSceneNumber);
            GameManager.Instance.PauseGame(false);
        }
    }

    /// <summary>
    /// Carrega a cena pelo índice do Build Settings
    /// </summary>
    /// <param name="sceneIndex">Número do índice do Build Settings</param>
    public void LoadScene(int sceneIndex)
    {
        //#if UNITY_EDITOR
        Debug.Log("Carregando cena: " + sceneIndex);
        //#endif
        SceneManager.LoadScene(sceneIndex);

    }
    /// <summary>
    /// Carrega a cena pelo nome da cena no Build Settings
    /// </summary>
    /// <param name="sceneName">Nome da cena no Build Settings</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Carrega a cena pelo SceneField
    /// </summary>
    /// <param name="sceneAsset">Asset da cena</param>
    public void LoadScene(SceneField sceneAsset)
    {
        SceneManager.LoadScene(sceneAsset);
    }


    void OnEnable()
    {
        // Diz ao método OnLevelFinishedLoading para atender um chamado de troca de cena
        SceneManager.sceneLoaded += OnLevelFinshedLoading;
    }

    void OnDisable()
    {
        // Diz ao método OnLevelFinishedLoading para deixar de atender um chamado assim que esse script for desabilitado
        SceneManager.sceneLoaded -= OnLevelFinshedLoading;
    }

    private void OnLevelFinshedLoading(Scene scene, LoadSceneMode mode)
    {
#if UNITY_EDITOR
        Debug.Log("Cena carregada: " + scene.name + ", modo: " + mode);
#endif
        //Toca a música referente a cena
        UIManager.OnLanguageChanged();
        persistentCamera.SetActive(false);
        //SoundManager.Instance.PlayMusic(scene.buildIndex);
    }

    /// <summary>
    /// Carrega a cena com fade-out pelo nome.
    /// </summary>
    /// <param name="sceneName">Nome da cena no Build Settings</param>
    public void FadeAndLoadScene(string sceneName)
    {
        if (!Extensions.isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }

    /// <summary>
    /// Carrega a cena com fade-out pelo número.
    /// </summary>
    /// <param name="sceneNumber">Numero da cena pelo build settings</param>
    public void FadeAndLoadScene(int sceneNumber)
    {
        if (!Extensions.isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(SceneManager.GetSceneByBuildIndex(sceneNumber).ToString()));
        }
    }


    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Extensions.FadeCanvas(faderCanvasGroup, 1f));

        //if (BeforeSceneUnload != null)
        //    BeforeSceneUnload();

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        //if (AfterSceneLoad != null)
        //    AfterSceneLoad();

        persistentCamera.SetActive(false);

        yield return StartCoroutine(Extensions.FadeCanvas(faderCanvasGroup, 0f));
    }

    /// <summary>
    /// Carrega uma cena em segundo plano
    /// </summary>
    /// <param name="sceneName">Nome da cena</param>
    /// <returns></returns>
    public IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        persistentCamera.SetActive(true);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    /// <summary>
    /// Carrega uma cena em segundo plano
    /// </summary>
    /// <param name="sceneAsset">Asset da cena</param>
    /// <returns></returns>
    public IEnumerator LoadSceneAndSetActive(SceneField sceneAsset)
    {
        persistentCamera.SetActive(true);
        yield return SceneManager.LoadSceneAsync(sceneAsset, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

}