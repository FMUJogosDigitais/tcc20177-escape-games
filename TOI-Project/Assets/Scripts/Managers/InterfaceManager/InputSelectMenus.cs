using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

#pragma warning disable 414

public class InputSelectMenus : MonoBehaviour
{

    public enum MenuMode
    {
        StartMenu,
        OptionsMenu,
		PagesMenu
    }
    public MenuMode menuMode;

    [System.Serializable]
    public struct MenuList
    {
        public GameObject menuItem;
        public GameObject SelectedItem;
        public string hintKey;
        //public bool Disabled;
    }
    public MenuList[] menuItems;

    public string defaultHintKey;
    public GameObject hintPanel;

    bool skipDisabledMaps = true;

    [SerializeField]
    private EventSystem eventSystem;
    [SerializeField]
    private GameObject selectedButton;
    [SerializeField]
    private GameObject cancelButton;
    [SerializeField]
    private GameObject confirmButton;

    private SoundManager sounds;

    private void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        sounds = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        if (hintPanel == null)
            hintPanel = GameObject.Find("HintPanel");
    }

    private void Start()
    {
        if (menuItems[0].menuItem != null)
            selectedButton = menuItems[0].menuItem;
        eventSystem.SetSelectedGameObject(menuItems[0].menuItem);
        //menuItems[1].Disabled = true;

        switch (menuMode)
        {
            case MenuMode.StartMenu:
                UpdateStartMenu();
                break;
            case MenuMode.OptionsMenu:
                UpdateOptionsMenu();
                break;
			case MenuMode.PagesMenu:
				UpdatePagesMenu();
				break;
            default:
                break;
        }



        //Debug.Log(playerInput.controllers.maps.GetFirstElementMapWithAction("UIVertical", skipDisabledMaps).elementIdentifierName);

    }

    /// <summary>
    /// Atualiza os items do menu inicial
    /// </summary>
    private void UpdateStartMenu()
    {
        foreach (MenuList item in menuItems)
        {
            if (eventSystem.currentSelectedGameObject == item.menuItem)
            {
                item.menuItem.GetComponentInChildren<TextMeshProUGUI>().fontSize = 90f;
                selectedButton = item.menuItem;
				if (item.SelectedItem != null)
				{
					item.SelectedItem.SetActive(true);
					item.SelectedItem.GetComponent<Image>().color = new Color(1f, 0.478f, 0.478f, 1f);
				}
				UpdateHintPanel(item.hintKey);
            }
            else
            {
                item.menuItem.GetComponentInChildren<TextMeshProUGUI>().fontSize = 72f;
				if (item.SelectedItem != null)
				{
					item.SelectedItem.GetComponent<Image>().color = new Color(1f, 1f, 1f, .2f);
					item.SelectedItem.SetActive(false);
				}
            }

        }
    }

    private void UpdateOptionsMenu()
    {
        foreach (MenuList item in menuItems)
        {
            if (eventSystem.currentSelectedGameObject == item.menuItem)
            {
                selectedButton = item.menuItem;
                if (item.SelectedItem != null)
                    item.SelectedItem.GetComponent<TextMeshProUGUI>().color = new Color(1f, 0.478f, 0.478f, 1f);
                //Debug.Log(item.SelectedItem.GetComponent<TextMeshProUGUI>().text);
                UpdateHintPanel(item.hintKey);
            }
            else
            {
                if (item.SelectedItem != null)
                    item.SelectedItem.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

	private void UpdatePagesMenu()
	{
		foreach (MenuList item in menuItems)
		{
			if (eventSystem.currentSelectedGameObject == item.menuItem)
			{
				selectedButton = item.menuItem;
				if (item.SelectedItem != null)
					item.SelectedItem.GetComponent<Image>().color = new Color(1f, 0.478f, 0.478f, 1f);
				//Debug.Log(item.SelectedItem.GetComponent<TextMeshProUGUI>().text);
				UpdateHintPanel(item.hintKey);
			}
			else
			{
				if (item.SelectedItem != null)
					item.SelectedItem.GetComponent<Image>().color = new Color(1f, 1f, 1f, .2f);
			}
		}
	}


	/// <summary>
	/// Atualiza a dica no painel de dicas
	/// </summary>
	/// <param name="hintKey"></param>
	private void UpdateHintPanel(string hintKey)
    {

        if (hintKey != "") // se existe uma dica no item
        {
            hintPanel.SetActive(true);
            hintPanel.GetComponentInChildren<TextMeshProUGUI>().spriteAsset = ControlIDManager.Instance.GetControllerGlyphys();
            hintPanel.GetComponentInChildren<TextMeshProUGUI>().text = ControlIDManager.Instance.GetTranslationWithKeys(hintKey);
        }
        else if (defaultHintKey != "") // pega a dica padrão caso exista
        {
            hintPanel.SetActive(true);
            hintPanel.GetComponentInChildren<TextMeshProUGUI>().spriteAsset = ControlIDManager.Instance.GetControllerGlyphys();
            hintPanel.GetComponentInChildren<TextMeshProUGUI>().text = ControlIDManager.Instance.GetTranslationWithKeys(defaultHintKey);
        }
        else // se não tiver então remove o painel
        {
            if (hintPanel != null)
                hintPanel.SetActive(false);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0 | Input.GetAxisRaw("Horizontal") != 0)
        {
            switch (menuMode)
            {
                case MenuMode.StartMenu:
                    UpdateStartMenu();
                    break;
                case MenuMode.OptionsMenu:
                    UpdateOptionsMenu();
                    break;
				case MenuMode.PagesMenu:
					UpdatePagesMenu();
					break;
                default:
                    break;
            }
        }

        if (cancelButton != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                sounds.PlayUISfx("UI_Cancel");
                cancelButton.GetComponent<Button>().onClick.Invoke();
            }
        }

        if (confirmButton != null)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                sounds.PlayUISfx("UI_Confirm");
                confirmButton.GetComponent<Button>().onClick.Invoke();
            }
        }


    }


    private void OnEnable()
    {
        Start();
    }


    public void SetSelectedButton(GameObject button)
    {
        selectedButton = button;
    }
}
