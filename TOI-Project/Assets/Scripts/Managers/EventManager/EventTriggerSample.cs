using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerSample : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.TriggerEvent("test");
        }
    }
}
