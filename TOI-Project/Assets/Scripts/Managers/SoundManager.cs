using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Classe sound manager gerencia os efeitos sonoros e a música conforme o modelo IEZA
/// </summary>
public class SoundManager : MonoBehaviour
{

    public AudioClip[] soundsEffect;               // lista de sound clips
    public AudioClip[] soundsInterface;            // lista de sound clips
    public AudioClip[] ambienceZone;               // lista de ambience clips
    public AudioClip[] sceneMusic;                 // lista de músicas

    public AudioSource interfaceAudio;             // componente AudioSource para tocar efeitos sonoros de interface
    public AudioSource effectAudio;                // componente AudioSource para tocar efeitos sonoros
    public AudioSource ambienceAudio;              // componente AudioSource para tocar sons de ambience
    public AudioSource musicAudio;                 // componente AudioSource para tocar música

    public AudioMixer masterMixer;                 //Asset AudioMixer 

    public Slider musicSlider;
    public Slider sfxSlider;

    private float sfxVolume;                       // Volume Efeitos Sonoros
    private float musicVolume;                     // Volume Música

    private static SoundManager soundManager;       // instância SoundManager

    //Inicio do Singleton
    private static SoundManager instance = null;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }//Fim do Singleton

        soundManager = this;

        // effectAudio = gameObject.AddComponent<AudioSource>();
        effectAudio.playOnAwake = false;
        effectAudio.loop = false;


        //  interfaceAudio = gameObject.AddComponent<AudioSource>();
        interfaceAudio.playOnAwake = false;
        interfaceAudio.loop = false;

        // musicAudio = gameObject.AddComponent<AudioSource>();
        musicAudio.playOnAwake = false;
        musicAudio.loop = true;

    }

    void Start()
    {
        // Lê os valores de volume do PlayerPrefs
        GetVolumeValues();

        //Registrando as notificações de pausa e resume
        //NotificationCenter.AddObserver(this, "Pause");
        GameManager.Instance.PauseNotification += Pause;
        //NotificationCenter.AddObserver(this, "Resume");
        GameManager.Instance.ResumeNotification += Resume;
    }

    public void Pause()
    {
        //PauseMusic(0.3f);
        masterMixer.SetFloat("musicVol", -80f);
        masterMixer.SetFloat("diegeticVol", -80f);
    }

    public void Resume()
    {
        //ResumeMusic();
        masterMixer.SetFloat("musicVol", 0f);
        masterMixer.SetFloat("diegeticVol", 0f);
    }

    /// <summary>
    /// Toca um soundclip pelo nome
    /// </summary>
    /// <param name="sfxName">O nome do soundclip a ser tocado</param>
    public static void PlaySfx(string sfxName)
    {
        if (soundManager == null)
        {
            Debug.LogWarning("Tentando tocar um som sem SoundManager na cena");
            return;
        }

        soundManager.PlaySound(sfxName, soundManager.soundsEffect, soundManager.effectAudio);
    }

    /// <summary>
    /// Toca um sound clip	
    /// </summary>
    /// <param name="clip">O sound clip a tocar</param>
    public static void PlaySfx(AudioClip clip)
    {
        soundManager.PlaySound(clip, soundManager.effectAudio);
    }

    /// <summary>
    /// Toca um efeito sonoro da UI
    /// </summary>
    /// <param name="sfxName"></param>
    public void PlayUISfx(string sfxName)
    {
        if (soundManager == null)
        {
            Debug.LogWarning("Tentando tocar um som sem SoundManager na cena");
            return;
        }

        soundManager.PlaySound(sfxName, soundManager.soundsInterface, soundManager.interfaceAudio);
    }

    /// <summary>
    /// Toca um efeito sonoro da UI
    /// </summary>
    /// <param name="clip"></param>
    public void PlayUISfx(AudioClip clip)
    {
        soundManager.PlaySound(clip, soundManager.interfaceAudio);
    }
    /// <summary>
    /// Começa a tocar uma música do começo
    /// </summary>
    /// <param name="trackName">Nome da música</param>
    public static void PlayMusic(string trackName)
    {
        if (soundManager == null)
        {
            Debug.LogWarning("Tentando tocar um som sem SoundManager na cena");
            return;
        }

        // reseta música para o começo
        soundManager.musicAudio.time = 0.0f;
        soundManager.musicAudio.volume = 1f;

        soundManager.PlaySound(trackName, soundManager.sceneMusic, soundManager.musicAudio);
    }

    /// <summary>
    /// Toca a música pelo índice da build number
    /// </summary>
    /// <param name="sceneNumber"></param>
    public void PlayMusic(int sceneNumber)
    {
        // reseta música para o começo
        musicAudio.time = 0.0f;
        musicAudio.volume = 1f;

        PlaySound(sceneNumber, sceneMusic, musicAudio);
        PlaySound(sceneNumber, ambienceZone, ambienceAudio);
    }

    /// <summary>
    /// Pausa a música.
    /// </summary>
    /// <param name="fadeTime">Tempo de fade out</param>
    public static void PauseMusic(float fadeTime)
    {
        if (fadeTime > 0.0f)
            soundManager.StartCoroutine(soundManager.FadeMusicOut(fadeTime));
        else
        {
            soundManager.musicAudio.Pause();
            soundManager.ambienceAudio.Pause();


        }
    }

    /// <summary>
    /// Despausa a música
    /// </summary>
    public static void ResumeMusic()
    {
        soundManager.musicAudio.volume = 1f;
        soundManager.musicAudio.UnPause();
        soundManager.ambienceAudio.UnPause();
    }

    /// <summary>
    /// Toca um som para um AudioSource
    /// </summary>
    private void PlaySound(string soundName, AudioClip[] pool, AudioSource audioOut)
    {
        // faz um loop pela nossa lista de clips até encontrar o correto.
        foreach (AudioClip clip in pool)
        {
            if (clip.name == soundName)
            {
                PlaySound(clip, audioOut);
                return;
            }
        }

        Debug.LogWarning("Nenhum clip de som encontrado com o nome " + soundName);
    }

    /// <summary>
    /// Toca um som para um AudioSource dado um índice no array
    /// </summary>
    private void PlaySound(int soundIndex, AudioClip[] pool, AudioSource audioOut)
    {
        if (pool == null) return;

        if (soundIndex > pool.Length) return;
        PlaySound(pool[soundIndex], audioOut);
    }

    /// <summary>
    /// Toca um som para um AudioSource dado
    /// </summary>
    private void PlaySound(AudioClip clip, AudioSource audioOut)
    {
        audioOut.clip = clip;
        audioOut.Play();
    }


    /// <summary>
    /// Co-Routina para fazer fade out da música
    /// </summary>
    /// <param name="time">tempo de fade</param>
    IEnumerator FadeMusicOut(float time)
    {
        float startVol = musicAudio.volume;
        float startTime = Time.realtimeSinceStartup;

        while (true)
        {
            // usar realtimeSinceStartup porque Time.time não aumenta quando o jogo é pausado.
            float t = (Time.realtimeSinceStartup - startTime) / time;
            if (t < 1.0f)
            {
                musicAudio.volume = (1.0f - t) * startVol;
                yield return 0;
            }
            else
            {
                break;
            }
        }

        // uma vez que fez o fade out, pausar a música
        musicAudio.Pause();
    }

    /// <summary>
    /// Atualiza o valor do volume dos efeitos sonoros
    /// </summary>
    /// <param name="volume"></param>
    public void SetSfxLevel(float volume)
    {
        volume *= 5;
        masterMixer.SetFloat("sfxVol", volume);
        sfxVolume = volume;
    }

    /// <summary>
    /// Atualiza o valor do volume da música
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicLevel(float volume)
    {
        volume *= 5;
        masterMixer.SetFloat("musicVol", volume);
        musicVolume = volume;
    }

    /// <summary>
    /// Pega ou reseta os valores de volume.
    /// </summary>
    public void GetVolumeValues()
    {
        sfxVolume = PlayerPrefsManager.GetSfxVolume();
        musicVolume = PlayerPrefsManager.GetMusicVolume();
        masterMixer.SetFloat("sfxVol", sfxVolume);
        masterMixer.SetFloat("musicVol", musicVolume);
        if (sfxSlider != null) sfxSlider.value = sfxVolume / 5; else Debug.LogWarning("sfxSlider não encontrado");
        if (musicSlider != null) musicSlider.value = musicVolume / 5; else Debug.LogWarning("musicSlider não encontrado");
    }

    /// <summary>
    /// Salva os valores de volume do audio.
    /// </summary>
    public void SaveVolumeChanges()
    {
        masterMixer.SetFloat("sfxVol", sfxVolume);
        masterMixer.SetFloat("musicVol", musicVolume);
        PlayerPrefsManager.SetSfxVolume(sfxVolume);
        PlayerPrefsManager.SetMusicVolume(musicVolume);
    }


}
