using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using Badin;
using System;
using System.Text.RegularExpressions;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI subtitle;
    [SerializeField] private CanvasGroup subtitleBox;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private CanvasGroup skipBox;
    [SerializeField] private TextMeshProUGUI skipText;

    [SerializeField] bool useTranslationKey = false;
    [SerializeField] string translationKey = "CUTSCENE_INTRO";

    private string[] fileLines;

    private List<string> subtitleLines = new List<string>();
    private List<string> subtitleTimingStrings = new List<string>();
    public List<float> subtitleTimings = new List<float>();
    public List<string> subtitleText = new List<string>();

    private int nextSubtitle = 0;

    private string displaySubtitle;

    /// <summary>
    /// Retorna o tempo total do vídeo em segundos
    /// </summary>
    /// <param name="videoPlayer"></param>
    /// <returns></returns>
    public float Duration(VideoPlayer videoPlayer)
    {
        return (videoPlayer.frameCount / videoPlayer.frameRate);
    }


    /// <summary>
    /// Retorna a porcentagem tempo do vídeo atual de 0 a 1
    /// </summary>
    /// <param name="videoPlayer"></param>
    /// <returns></returns>
    public Double NTime(VideoPlayer videoPlayer)
    {
        return (videoPlayer.time / Duration(videoPlayer));
    }


    #region Singleton
    //Inicio do Singleton
    private static CutsceneManager instance = null;
    public static CutsceneManager Instance // Esse script possuí uma referência estática a si mesmo para que os outros scripts possam acessar sem precisar de uma referência para tal.
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
        }//Fim do Singleton
        #endregion
        #region Awake
        // Demais scripts Awake
        #endregion  
    }

    private void Start()
    {
        if (GameManager.Instance) GameManager.Instance.CutsceneSkip += SkipCutscene;
        BeginCutscene();
    }

    private void BeginCutscene()
    {
        // Reset todas as linhas
        subtitleLines = new List<string>();
        subtitleTimingStrings = new List<string>();
        subtitleTimings = new List<float>();
        subtitleText = new List<string>();

        nextSubtitle = 0;

        TextAsset temp = Resources.Load("Dialogues/" + video.name) as TextAsset;
        fileLines = temp.text.Split('\n');


        foreach (string line in fileLines)
        {
            subtitleLines.Add(line);
        }

        for (int i = 0; i < subtitleLines.Count; i++)
        {
            string[] splitTemp = subtitleLines[i].Split('|');
            subtitleTimingStrings.Add(splitTemp[0]);
            subtitleTimings.Add(float.Parse(subtitleTimingStrings[i]));
            if (useTranslationKey)
            {
                subtitleText.Add(translationKey + "_" + (i + 1));
            }
            else
            {
                subtitleText.Add(splitTemp[1]);
            }

        }


        if (subtitleText[0] != null)
        {
            displaySubtitle = subtitleText[0];
        }

        video.Play();

    }

    private void PlaySubtitles()
    {
        if (nextSubtitle > 0 && !subtitleText[nextSubtitle - 1].Contains("<break/>"))
        {
            DisplaySubtitle(displaySubtitle);
        }
        else
        {
            subtitleBox.alpha = 0;
        }

        if (nextSubtitle < subtitleText.Count)
        {
            if (video.time > subtitleTimings[nextSubtitle])
            {
                displaySubtitle =  subtitleText[nextSubtitle];
                nextSubtitle++;
            }
        }
        else if (NTime(video) >= 0.99)
        {
            SkipCutscene();
        }


    }

    private void Update()
    {
        PlaySubtitles();
    }

    private void DisplaySubtitle(string message)
    {
        subtitleBox.alpha = 1;
        subtitle.text = message;

    }

    //private string CleanTimeString(string timeString)
    //{
    //    Regex digitsOnly = new Regex(@"[^\d+(\.+)*$]");
    //    return digitsOnly.Replace(timeString, "");
    //}

    private void SkipCutscene()
    {
        if (!GameManager.Instance)
        {
#if UNITY_EDITOR 
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            GameManager.Instance.CutsceneSkip -= SkipCutscene;
            GameManager.Instance.Goto_MainGame();
        }
    }


}
