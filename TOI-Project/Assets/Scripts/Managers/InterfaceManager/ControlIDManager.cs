using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

#pragma warning disable 649
[DisallowMultipleComponent]
public class ControlIDManager : MonoBehaviour
{
    [SerializeField]
    private ControllerEntry[] controllers;

    [System.Serializable]
    private class ControllerEntry
    {
        public string name;
        public TMP_SpriteAsset SpriteAsset;
    }

    public TMP_SpriteAsset keyboardSpriteAsset;
    public TMP_SpriteAsset genericControllerSpriteAsset;
    private TMP_SpriteAsset currentSpriteAsset;

    #region Singleton
    //Inicio do Singleton
    private static ControlIDManager instance = null; 
    public static ControlIDManager Instance // Esse script possuí uma referência estática a si mesmo para que os outros scripts possam acessar sem precisar de uma referência para tal.
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

    /// <param name="key"></param>
    /// <returns></returns>
    public string GetTranslationWithKeys(string key)
    {
			return key;
    }

    /// <summary>
    /// Pega o spriteAsset referente ao controle atual
    /// </summary>
    /// <returns>SpriteAsset</returns>
    public TMP_SpriteAsset GetControllerGlyphys()
    {

        return currentSpriteAsset;
    }

}
