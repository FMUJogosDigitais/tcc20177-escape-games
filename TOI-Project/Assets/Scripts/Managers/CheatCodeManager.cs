using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Classe que gerencia cheat codes.
/// </summary>
public class CheatCodeManager : MonoBehaviour
{
    public Keychain chaveiro;
    [Header("CHEAT CODES")]

    //array de todos os cheatcodes que queremos no jogo
    [SerializeField]
    string[] targetCode;

    [SerializeField]
    List<string> inputCode = new List<string>();
    string currentCode;

    void Update()
    {
        if (Input.anyKeyDown)
        {

            //
            //código da teclas direcionais
            //up = ^
            //down = .
            //left = < 
            //right = > 
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputCode.RemoveAt(0);
                inputCode.Add("^");
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputCode.RemoveAt(0);
                inputCode.Add(".");

            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputCode.RemoveAt(0);
                inputCode.Add("<");

            }

            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputCode.RemoveAt(0);
                inputCode.Add(">");

            }

            //se for letra
            else if (Regex.IsMatch(Input.inputString, @"[a-zA-Z0-9]"))
            {
                inputCode.RemoveAt(0);
                inputCode.Add(Input.inputString);
            }

            //apaga a string atual
            currentCode = "";

            //preenche a string do código  com todos os inputs anteriores
            for (int i = 0; i < inputCode.Count; i++)
            {
                currentCode += inputCode[i];
            }

            //verifica se o código atual bate com qualquer um dos cheats
            for (int i = 0; i < targetCode.Length; i++)
            {
                if (currentCode.Contains(targetCode[i].ToLower()))
                {
					ExecuteCode(targetCode[i]);
                    for (int j = 0; j < inputCode.Count; j++)
                    {
                        inputCode[j] = null;
                    }
                }
            }
        }
    }

    void ExecuteCode(string code)
    {
        switch (code)
        {
            case "chaves":
				chaveiro.AddAllKeys();
                break;
			case "chave0":
				chaveiro.InitKeys();
				break;
			case "chave1":
				chaveiro.AddKey(0);
				break;
			case "chave2":
				chaveiro.AddKey(1);
				break;
			case "chave3":
				chaveiro.AddKey(2);
				break;
			case "chave4":
				chaveiro.AddKey(3);
				break;
			case "chave5":
				chaveiro.AddKey(4);
				break;
			case "morreu":
				PlayerSanity.Instance.Sanity = 1;
				break;


				//adicionar um novo case para cada código
		}
	}
}