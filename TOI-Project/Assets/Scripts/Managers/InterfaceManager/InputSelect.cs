using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

#pragma warning disable 414

public class InputSelect : MonoBehaviour
{

	[System.Serializable]
	public struct MenuList
	{
		public GameObject menuItem;
		public GameObject SelectedItem;
	}
	public MenuList[] menuItems;

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
		eventSystem = GameObject.Find("RewiredEventSystem").GetComponent<EventSystem>();
	}

	private void Start()
	{
		if (menuItems[0].menuItem != null)
			selectedButton = menuItems[0].menuItem;
		eventSystem.SetSelectedGameObject(menuItems[0].menuItem);
		//menuItems[1].Disabled = true;

		UpdateMenu();

	}


	/// <summary>
	/// Atualiza os items do menu inicial
	/// </summary>
	private void UpdateMenu()
	{
		foreach (MenuList item in menuItems)
		{
			if (eventSystem.currentSelectedGameObject == item.menuItem)
			{
				selectedButton = item.menuItem;
				if (item.SelectedItem != null)
					item.SelectedItem.SetActive(true);
			}
			else
			{
				if (item.SelectedItem != null)
					item.SelectedItem.SetActive(false);
			}

		}

	}
	void Update()
	{
		if (Input.GetAxisRaw("Vertical") != 0 | Input.GetAxisRaw("Horizontal") != 0)
		{
			UpdateMenu();
		}


		if (cancelButton != null)
		{
			if (Input.GetButtonDown("UICancel"))
			{
				cancelButton.GetComponent<Button>().onClick.Invoke();
			}
		}

		if (confirmButton != null)
		{
			if (Input.GetButtonDown("Get"))
			{
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
#pragma warning restore 414
