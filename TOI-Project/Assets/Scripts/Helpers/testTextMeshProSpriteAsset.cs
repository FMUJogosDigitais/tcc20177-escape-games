using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class testTextMeshProSpriteAsset : MonoBehaviour
{

    TextMeshProUGUI text;
    private int lenght;

    private void Start()
    {

        text = GetComponent<TextMeshProUGUI>();
        lenght = text.spriteAsset.spriteInfoList.Capacity;

        string texto = "";

        for (int i = 0; i < lenght; i++)
        {
            texto += "_<sprite=" + i + ">";
        }

        text.text = texto;
    }
}
