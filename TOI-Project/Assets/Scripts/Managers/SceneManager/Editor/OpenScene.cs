using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneMenu : Editor
{

    [MenuItem("Jogo/Início do Jogo", false, 1)]
    public static void StartGame()
    {
        OpenScene("Persistent");
    }


    [MenuItem("Jogo/Tela de Título", false, 51)]
    public static void TitleScreen()
    {
        OpenScene("StartScreen");
    }

    [MenuItem("Jogo/Tela do Menu Inicial", false, 52)]
    public static void MenuScreen()
    {
        OpenScene("StartMenu");
    }


    [MenuItem("Jogo/Cutscene Introdução", false, 101)]
    public static void CutsceneIntro()
    {
        OpenScene("CutsceneIntro");
    }


    [MenuItem("Jogo/Cenário", false, 102)]
    public static void Scenery()
    {
        OpenScene("Cenario");
    }


    //[MenuItem("Jogo/Cena de Testes", false, 151)]
    //public static void TestScene()
    //{
    //    OpenScene("Testes");
    //}

    static void OpenScene(string name)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }
}