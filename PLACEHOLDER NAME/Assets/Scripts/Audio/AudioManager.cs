using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource backgroundMusicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioSource _BackgroundMusicSource => backgroundMusicSource; // Read-only
    public AudioSource _SFXSource => SFXSource; // Read-only
    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip playerHit;
    public AudioClip fireball;
    public AudioClip flask;
    public AudioClip freezeWave;
    public AudioClip XPOrb;

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

    private void Start()
    {
        backgroundMusicSource.clip = background;
        backgroundMusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
