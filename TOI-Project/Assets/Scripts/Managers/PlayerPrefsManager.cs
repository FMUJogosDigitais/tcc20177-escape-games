using UnityEngine;
using Badin;

/// <summary>
/// Classe que centraliza as chaves a serem salvas no registro do dispositivo
/// </summary>
public class PlayerPrefsManager : MonoBehaviour
{

	const string MUSIC_VOLUME_KEY = "music_volume";
	const string SFX_VOLUME_KEY = "sfx_volume";
	const string UNLOCKED_PAGE_KEY = "unlocked_page";

	private static bool[] unlockedPage = new bool[UIManager.NUMBER_OF_PAGES];

	/// <summary>
	/// Salva o valor do volume de música
	/// </summary>
	/// <param name="volume">Volume da música</param>
	public static void SetMusicVolume(float volume)
	{
		if (volume >= -80f && volume <= 0f)
		{
			PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
		}
		else
		{
			Debug.LogError("Volume musica está fora dos limites");
		}
	}

	/// <summary>
	/// Lê o valor de volume da música
	/// </summary>
	/// <returns>Volume da música</returns>
	public static float GetMusicVolume()
	{
		return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0f);
	}

	/// <summary>
	/// Salva o valor do volume dos efeitos sonoros 
	/// </summary>
	/// <param name="volume">Volume SFX</param>
	public static void SetSfxVolume(float volume)
	{
		if (volume >= -80f && volume <= 0f)
		{
			PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
		}
		else
		{
			Debug.LogError("Volume SFX está fora dos limites");
		}
	}

	/// <summary>
	/// Lê o valor de volume dos efeitos sonoros
	/// </summary>
	/// <returns>O valor de volume SFX</returns>
	public static float GetSfxVolume()
	{
		return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0f);
	}

	/// <summary>
	/// Retorna se a página do diário está desbloqueada
	/// </summary>
	/// <param name="index">Índice no array</param>
	/// <returns></returns>
	public static bool GetPageUnlocked(int index)
	{
		unlockedPage = Extensions.IntToBoolArray(PlayerPrefs.GetInt(UNLOCKED_PAGE_KEY, 0), unlockedPage.Length);
		return unlockedPage[index];
	}

	/// <summary>
	/// Salva o desbloqueio da página do diário no índice
	/// </summary>
	/// <param name="index">Índice no Array</param>
	/// <param name="unlocked"></param>
	public static void SetPageUnlocked(int index, bool unlocked)
	{
		unlockedPage[index] = unlocked;
		PlayerPrefs.SetInt(UNLOCKED_PAGE_KEY, Extensions.BoolArrayToInt(unlockedPage));
	}
}