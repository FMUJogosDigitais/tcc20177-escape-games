using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Badin;

public class PlayerSanity : MonoBehaviour
{
	[SerializeField] private float speedDecrease = 0.01f;
	[SerializeField] private float speedIncrease = 30f;
	[SerializeField] private float m_Sanity = 100;

	public AudioClip[] scarySounds;
	public AudioSource scaryAudioSource;

	public AudioClip[] thunderSounds;
	public AudioSource thunderAudioSource;
	public Light lightning;

	public float Sanity
	{
		get
		{
			if (m_Sanity <= 0)
			{
				//Recovery(1f);
				return 0;
			}
			else if (m_Sanity >= 100f)
			{
				return 100f;
			}
			else
			{
				return m_Sanity;
			}
		}
		set
		{
			if (value <= 0f)
			{
				m_Sanity = 0f;
			}
			else if (value >= 100f)
			{
				m_Sanity = 100f;
				isRecovering = false;
				//isScared = false;
			}
			else
			{
				m_Sanity = value;
			}
		}
	}

	public string Timer
	{
		get
		{
			return timerText.text;
		}
	}

	public int Seconds
	{
		get
		{
			return Mathf.RoundToInt(gameClock);
		}
	}

	public bool isRecovering = false;
	public bool isScared = false;

	public AudioClip soundBreath;
	public AudioClip soundHeartBeat;
	public AudioSource audioBreath;
	public AudioSource audioHeartBeat;
	public AudioSource audioRain;
	
	public Text timerText;
	public Text sanityText;
	private float gameClock = 0;
	private bool isCounting;
	private bool timerStarted = false;
	#region Singleton
	//Início do Singleton
	public static PlayerSanity Instance;
	void Awake()
	{
		if (Instance == null)
			Instance = this;

		else if (Instance != this)
			Destroy(this);
		//Fim do Singleton
		#endregion
		#region Awake
		// Scripts do Awake
		#endregion
	}

	private void Start()
	{
		audioBreath.clip = soundBreath;
		audioHeartBeat.clip = soundHeartBeat;
		audioBreath.Play();
		audioHeartBeat.Play();


		timerText.text = "0:00";

#if !UNITY_EDITOR
		timerText.enabled = false;
#else
		timerText.enabled = true;
#endif
	}

	public void StartTimer()
	{
		if (timerStarted) return;
		ResetTimer();
	}

	private void ResetTimer()
	{
		timerStarted = true;
		gameClock = Time.time;
		gameClock -= gameClock;
		isCounting = true;
		Invoke("AudioRain", 1f);
		InvokeRepeating("LightningsAndThunders", 30f, 60f);
	}

	private void AudioRain()
	{
		audioRain.Play();
	}

	private void Update()
	{
		ClockUpdate();
		SanityUpdate();
	}

	float difference;

	private void SanityUpdate()
	{
		if (!isScared) return;
		difference = 100f - Sanity;

		if (Sanity <= 40)
		{
			audioBreath.volume = difference / 100f;
			audioHeartBeat.volume = difference / 100f;
		}
		else
		{
			audioBreath.volume -= Time.deltaTime;
			audioHeartBeat.volume -= Time.deltaTime;
		}

		if (!isRecovering)
			Sanity -= Time.deltaTime * speedDecrease;
		else
			Sanity += Time.deltaTime * speedIncrease;

		sanityText.text = "Sanity: "+ m_Sanity.ToString("0.00");

		if (Sanity <= 0)
		{
			GameOver();
		}

	}

	bool gameover = false;

	private void GameOver()
	{
		if (gameover) return;
		gameover = true;
		if (GameManager.Instance)
		{
			GameManager.Instance.uiManager.ShowScreen("GameOverPanel");
			GameplayUIManager.Instance.ShowScreen("");
			GameplayUIManager.Instance.HoldGameplay();
		}
	}

	public void Scare(float value)
	{
		isScared = true;
		Sanity -= value;
		StartTimer();
		scaryAudioSource.clip = scarySounds[UnityEngine.Random.Range(0, scarySounds.Length)];
		if (scaryAudioSource.clip != null) scaryAudioSource.Play();

		Invoke("LightningsAndThunders", 1f);
		Recovery(value / 10);
	}

	public void Recovery(float time)
	{
		if (isRecovering) return;
		StartCoroutine(RecoveryBy(time));
	}

	private IEnumerator RecoveryBy(float v)
	{
		isRecovering = true;
		yield return new WaitForSeconds(v);
		isRecovering = false;
	}

	private void ClockUpdate()
	{
		if (!isCounting) return;
		gameClock += Time.deltaTime;
		string clockText = Extensions.FloatToTime(gameClock, "#0:00");
		timerText.text = clockText;
	}

	void LightningsAndThunders()
	{
		StartCoroutine(Lightings());
		Invoke("Thunder", 1f);
	}

	private void Thunder()
	{
		thunderAudioSource.clip = thunderSounds[UnityEngine.Random.Range(0, thunderSounds.Length)];
		if (thunderAudioSource.clip != null) thunderAudioSource.Play();
	}

	private WaitForSeconds wait = new WaitForSeconds(0.1f);
	[HideInInspector] public bool noLightnings = false;

	private IEnumerator Lightings()
	{
		if (noLightnings) yield break;

		for (int i = 0; i < 7; i++)
		{
			lightning.intensity = 2;
			yield return wait;
			lightning.intensity = 0;
			yield return wait;
		}
	}


}
