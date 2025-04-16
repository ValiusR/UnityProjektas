using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource backgroundMusicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioSource _BackgroundMusicSource => backgroundMusicSource; // Read-only
    public AudioSource _SFXSource => SFXSource; // Read-only
    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip bossBattle;
    public AudioClip playerHit;
    public AudioClip fireball;
    public AudioClip flask;
    public AudioClip freezeWave;
    public AudioClip XPOrb;

    private float backgroundMusicTime = 0f;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Just for testing when launching game from Main
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            backgroundMusicSource.clip = background;
            backgroundMusicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayBossMusic()
    {
        backgroundMusicTime = backgroundMusicSource.time;
        StartCoroutine(FadeAndSwitchMusic(bossBattle));
    }

    public void ResumeBackgroundMusic()
    {
        StartCoroutine(FadeAndSwitchMusic(background, resumeTime: backgroundMusicTime));
    }


    private IEnumerator FadeAndSwitchMusic(AudioClip newClip, float fadeDuration = 1.5f, float resumeTime = 0f)
    {
        float startVolume = backgroundMusicSource.volume;

        // Fade out
        while (backgroundMusicSource.volume > 0)
        {
            backgroundMusicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Switch clip
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = newClip;
        backgroundMusicSource.time = resumeTime;
        backgroundMusicSource.Play();

        // Fade in
        while (backgroundMusicSource.volume < startVolume)
        {
            backgroundMusicSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusicSource.volume = startVolume;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            StartCoroutine(FadeAndSwitchMusic(background));
        }
    }
}
