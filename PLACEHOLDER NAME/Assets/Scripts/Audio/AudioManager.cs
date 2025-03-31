using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource backgroundMusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    [SerializeField] AudioClip background;

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
}
