using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnInput : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject cancelButton;

    private SoundManager sounds;

    public bool buttonSelected = false;

    void Start()
    {
        eventSystem = GameObject.Find("RewiredEventSystem").GetComponent<EventSystem>();
        sounds = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        buttonSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("UIVertical") != 0 && buttonSelected == false )
        {
            if (selectedObject != eventSystem.currentSelectedGameObject)
            {
                eventSystem.SetSelectedGameObject(selectedObject);
                buttonSelected = true;
            }
        }

        if (Input.GetButtonDown("UICancel"))
        {
            eventSystem.SetSelectedGameObject(cancelButton);
            sounds.PlayUISfx("UI_Cancel");
            cancelButton.GetComponent<Button>().onClick.Invoke();
            buttonSelected = false;
        }
    }

 
    private void OnDisable()
    {
        buttonSelected = false;
    }

}
