using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
	public const int NUM_SWITCHES = 3;
	public GeneratorSwitch[] switches = new GeneratorSwitch[NUM_SWITCHES];
	public GeneratorLever[] levers = new GeneratorLever[NUM_SWITCHES];
	public SwitchState[] switchState = new SwitchState[NUM_SWITCHES];

	public string binaryAnswer; //Resposta atual em binário
	public int numericAnswer; // Resposta atual em decimal
	public string binaryExpected; // Resposta esperada em binário
	public int numericExpected; // Resposta esperada em decimal

	private AudioSource generatorSound;

	private WaitForSeconds wait0_1 = new WaitForSeconds(.1f);

	#region Singleton
	//Inicio do Singleton
	private static Generator instance = null;
	public static Generator Instance // Esse script possuí uma referência estática a si mesmo para que os outros scripts possam acessar sem precisar de uma referência para tal.
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
		numericExpected = UnityEngine.Random.Range(0, 8); // Sorteia de 0 a 7
		binaryExpected = Convert.ToString(numericExpected, 2); //Converte o número sorteado para binário
		generatorSound = GetComponent<AudioSource>();
	}

	public void SetSwitch(GeneratorLever lever, SwitchState state)
	{
		for (int i = 0; i < NUM_SWITCHES; i++)
		{
			if (levers[i] == lever)
			{
				switchState[i] = state;
				continue;
			}
		}
		UpdateAnswer();
	}

	private void UpdateAnswer()
	{
		for (int i = 0; i < NUM_SWITCHES; i++)
		{
			if (switchState[i] == SwitchState.Null) return;
		}

		binaryAnswer = "";

		for (int i = 0; i < NUM_SWITCHES; i++)
		{
			if (switchState[i] == SwitchState.Up) binaryAnswer += "1";
			else binaryAnswer += "0";
		}

		numericAnswer = Convert.ToInt32(binaryAnswer, 2);
		CheckAnswer();
	}

	private void CheckAnswer()
	{
		if (numericAnswer == numericExpected) StartCoroutine(AcertoMizeravi());
	}


	private IEnumerator AcertoMizeravi()
	{
		generatorSound.Play();
		for (int i = 0; i < 3; i++)
		{
			yield return wait0_1;
			LightsManager.Instance.Intensity = .1f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .2f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .3f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .4f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .5f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .6f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .7f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .8f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = .9f;
			yield return wait0_1;
			LightsManager.Instance.Intensity = 1f;
		}
		yield return wait0_1;
		LightsManager.Instance.RestoreLights();

		yield break;
	}
}
