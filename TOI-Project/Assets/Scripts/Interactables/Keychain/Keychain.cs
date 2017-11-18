using System;
using UnityEngine;

[DisallowMultipleComponent]
/// <summary>
/// Classe que gerencia as chaves das portas
/// </summary>
public class Keychain : MonoBehaviour
{
	public GameObject[] keyInv = new GameObject[NUM_KEY_SLOTS];
	public bool[] haveKey = new bool[NUM_KEY_SLOTS];
	public Door[] door = new Door[NUM_KEY_SLOTS];
	public GameObject[] keyObj = new GameObject[NUM_KEY_SLOTS];

	public const int NUM_KEY_SLOTS = 5;

	public string HaveKeys
	{
		get
		{
			return GetKeysString();
		}
	}

	private string GetKeysString()
	{
		string keysString = "";

		for (int i = 0; i < NUM_KEY_SLOTS; i++)
		{
			if (haveKey[i] == true) keysString = keysString + (i + 1);
			else keysString += "0";
		}

		return keysString;
	}

	#region Singleton
	//Inicio do Singleton
	private static Keychain instance = null;
	public static Keychain Instance // Esse script possuí uma referência estática a si mesmo para que os outros scripts possam acessar sem precisar de uma referência para tal.
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
		Init();
	}

	/// <summary>
	/// Quando inicializar, zera todas as chaves
	/// </summary>
	private void Init()
	{
		// Zera o haveKey em todas as chaves
		for (int i = 0; i < NUM_KEY_SLOTS; i++)
		{
			haveKey[i] = false;
		}

		InitKeys();
	}

	/// <summary>
	/// Inicializa o estado das chaves.
	/// </summary>
	public void InitKeys()
	{
		for (int i = 0; i < NUM_KEY_SLOTS; i++)
		{
			keyInv[i].SetActive(haveKey[i]);
			door[i].IsLocked = !haveKey[i];
			keyObj[i].SetActive(!haveKey[i]);
			door[i].doorKey = keyObj[i].GetComponent<Key>();
			if (door[i].rightDoor) door[i].rightDoor.doorKey = keyObj[i].GetComponent<Key>();
		}
	}

	/// <summary>
	/// Metodo público que adiciona a chave no inventário
	/// </summary>
	/// <param name="key"></param>
	public void AddKey(Key key)
	{
		Interactions.Instance.lastHighlighted = null; //Remove a chave do lasthighlighted
		for (int i = 0; i < NUM_KEY_SLOTS; i++)
		{
			if (keyObj[i].name == key.name)
			{
				haveKey[i] = true;
				keyInv[i].SetActive(haveKey[i]);
				door[i].IsLocked = !haveKey[i];
				keyObj[i].SetActive(!haveKey[i]);
				return;
			}
		}
	}

	public void AddKey(int numKey)
	{
		if (numKey >= NUM_KEY_SLOTS || numKey < 0)
		{
			Debug.LogError("Chave fora do limite");
			return;
		}
		AddKey(keyObj[numKey].GetComponent<Key>());
	}

	/// <summary>
	/// Remove a chave do inventário após usar na porta
	/// </summary>
	/// <param name="key"></param>
	public void UseKey(Key key)
	{
		for (int i = 0; i < NUM_KEY_SLOTS; i++)
		{
			if (keyObj[i].name == key.name)
			{
				haveKey[i] = false;
				keyInv[i].SetActive(haveKey[i]);
				return;
			}
		}
	}

	public void AddAllKeys()
	{
		for (int i = 0; i < NUM_KEY_SLOTS; i++)
		{
			haveKey[i] = true;
			keyInv[i].SetActive(haveKey[i]);
			door[i].IsLocked = !haveKey[i];
			keyObj[i].SetActive(!haveKey[i]);
		}
	}

}
