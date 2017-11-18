using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

#pragma warning disable 414

[RequireComponent(typeof(ScreenSettings))]
public class StaticTextManagerTMPro : MonoBehaviour
{

    ScreenSettings screenSettings;

    [System.Serializable]
    public struct TextInfo
    {
        public TextMeshProUGUI textTMPro;
        public string localizationKey;
        public bool textWithControlerKeys;
    }

    public TextInfo[] textObjects;

    private void Awake()
    {
        screenSettings = GetComponent<ScreenSettings>();
    }

    // Use this for initialization
    void Start()
    {
        UpdateText();
    }

    void UpdateText()
    {
        foreach (TextInfo t in textObjects)
        {
            if (t.textTMPro != null)
            {
				if (!t.textWithControlerKeys)
					t.textTMPro.text = t.localizationKey;
            }
            else
            {
                Debug.LogWarning("Objeto Texto " + t.localizationKey + " não encontrado.");
            }
        }
    }

    public void OnLanguageChanged()
    {
        UpdateText();
    }
}
