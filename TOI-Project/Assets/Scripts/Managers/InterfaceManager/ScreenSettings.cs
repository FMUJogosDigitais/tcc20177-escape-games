using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(GameManager))]
/// <summary>
/// Classe que gerencia as opções gráficas do jogo
/// </summary>
public class ScreenSettings : MonoBehaviour
{
 //   GameManager gameManager;


    public Resolution[] resolutions;
    [HideInInspector]
    public List<string> resolutionList;
    [HideInInspector]
    public List<int> resolutionWidthList;
    [HideInInspector]
    public List<int> resolutionHeightList;
    public string currentResolution;
    public string nextResolution;
    public int currentResolutionIndex;
    public int resolutionIndex;

    public Slider resolutionSlider;
    public TextMeshProUGUI SetResolutionLabelText;

    public Slider QualityLevelSlider;
    public TextMeshProUGUI QualityLevelStateLabel;

    //public Slider FOVLevelSlider;
    //public Text FOVLevelStateLabel;

    //public Slider BloomLevelSlider;
    //public Text BloomLevelStateLabel;

    // Resolução de tela mínima
    private const int LOWEST_RESOLUTION_WIDTH = 800;
    private const int LOWEST_RESOLUTION_HEIGHT = 600;

    // Chaves do registro das opções
    private const string GRAPHICS_LEVEL_KEY = "Graphics_Level";


    // Valores padrões das opções
    private const int GRAPHICS_LEVEL = 5;

    private void Awake()
    {
 //       gameManager = GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start()
    {
        GetValues(); //Inicializa a lista de resoluções
        StartCoroutine("LateStart");
    }
    IEnumerator LateStart()
    {
        yield return null;
        yield return new WaitForSeconds(0.1f);
        LoadPrefs(); //Carrega as preferências gráficas
        yield break;

    }

    /// <summary>
    /// Inicializa a lista de resoluções
    /// </summary>
    private void GetValues()
    {
        currentResolution = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
        resolutions = Screen.resolutions;
        string lastRes = "";
        string currRes = "";
        //Cria uma lista de resoluções a partir de 800x600;
        foreach (Resolution res in resolutions)
        {
            if (res.width >= LOWEST_RESOLUTION_WIDTH && res.height >= LOWEST_RESOLUTION_HEIGHT)
            {
                currRes = res.width + "x" + res.height;
                if (lastRes != currRes)
                {
                    lastRes = currRes;
                    resolutionList.Add(currRes);
                    resolutionWidthList.Add(res.width);
                    resolutionHeightList.Add(res.height);
                }
            }
        }

        currentResolutionIndex = GetResolutionIndex();
        resolutionIndex = currentResolutionIndex;
        SwitchResolutionIndex(resolutionIndex);

    }
    private int GetResolutionIndex()
    {
        //Anota a resolução atual
        for (int i = 0; i < resolutionList.Count; i++)
        {
            if (resolutionList[i] == currentResolution) { return i; }
        }
        return 0;
    }

    public void NextResolution()
    {
        resolutionIndex++;
        if (resolutionIndex >= resolutionList.Count) { resolutionIndex = 0; }
        SwitchResolutionIndex(resolutionIndex);
    }

    public void SetResolution()
    {
        SwitchResolutionIndex((int)resolutionSlider.value);
    }

    private void SwitchResolutionIndex(int resIdx)
    {
        resolutionIndex = resIdx;
        nextResolution = resolutionList[resIdx].ToString();
        if (SetResolutionLabelText != null)
        {
            SetResolutionLabelText.text = nextResolution;
        }
        else
        {
            Debug.LogWarning("Objeto resolutionButtonText não encontrado");
        }
    }

    public void PrevResolution()
    {
        resolutionIndex--;
        if (resolutionIndex < 0) { resolutionIndex = resolutionList.Count - 1; }
        SwitchResolutionIndex(resolutionIndex);
    }

    public void ConfirmSettings()
    {
        //Troca a resolução da tela
        Screen.SetResolution(resolutionWidthList[resolutionIndex], resolutionHeightList[resolutionIndex], Screen.fullScreen);
        //Salva as configurações
        SavePrefs();
    }

    public void CancelSettings()
    {
        //Carrega as configurações
        LoadPrefs();
        //Reseta os valores de resoluções
        //GetValues();
    }

    public void SetQuality()
    {
        int graphicSetting = Mathf.RoundToInt(QualityLevelSlider.value);
        if (graphicSetting > 0)
        {
            QualitySettings.SetQualityLevel(graphicSetting, true);
        }

        UpdateAllLabels();
    }


    /// <summary>
    /// Atualiza o texto dos rótulos do ScreenSettings
    /// </summary>
    public void UpdateAllLabels()
    {

        resolutionSlider.maxValue = resolutionList.Count - 1;
        resolutionSlider.value = currentResolutionIndex;

        if (resolutionSlider != null)
        {
            resolutionSlider.value = resolutionIndex;
        }

        if (QualityLevelSlider != null)
        {
            SetQualityStatusLabel(Mathf.RoundToInt(QualityLevelSlider.value));
        }
        else Debug.LogWarning("QualityLevelSlider não encontrado");

    }

    void SetQualityStatusLabel(int setting)
    {
        switch (setting)
        {
            case 0:
                QualityLevelStateLabel.text = "Automatic";
                break;
            case 1:
                QualityLevelStateLabel.text = "Low";
                break;
            case 2:
                QualityLevelStateLabel.text = "Medium";
                break;
            case 3:
                QualityLevelStateLabel.text = "High";
                break;
            case 4:
                QualityLevelStateLabel.text = "Very High";
                break;
            case 5:
                QualityLevelStateLabel.text = "Ultra";
                break;
        }
    }


    void SavePrefs()
    {
        PlayerPrefs.SetInt(GRAPHICS_LEVEL_KEY, Mathf.RoundToInt(QualityLevelSlider.value));
    }

    public void LoadPrefs()
    {
        if (QualityLevelSlider != null)
        {
            QualityLevelSlider.value = PlayerPrefs.GetInt(GRAPHICS_LEVEL_KEY, GRAPHICS_LEVEL);
        }
        else
        {
            Debug.LogWarning("QualityLevelSlider não encontrado");
            return;
        }


        //Seta os sliders
        SetQuality();
    }

    public void ResetSettings()
    {
        resolutionIndex = resolutionList.Count - 1;
        SwitchResolutionIndex(resolutionIndex);
        QualityLevelSlider.value = GRAPHICS_LEVEL;
        SetQuality();
    }
}
