using UnityEngine;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clip Names")]
    // Define SFX clip name constants for "shoot", "explosion", and "hit"
    public const int SFX_SHOOT = 0;
    public const int SFX_EXPLOSION = 1;
    public const int SFX_HIT = 2;

    // Define music clip name constants for "menu", "gameplay", and "boss"
    public const int MUSIC_MENU = 0;
    public const int MUSIC_GAMEPLAY = 1;
    public const int MUSIC_BOSS = 2;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;

    [Header("Audio Settings")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Set initial volumes
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    private void Start()
    {
        PlayMusic(MUSIC_GAMEPLAY);
    }

    public void PlayMusic(int index, bool loop = true)
    {
        if (musicClips == null || index < 0 || index >= musicClips.Length)
            return;

        if (musicSource != null)
        {
            musicSource.clip = musicClips[index];
            musicSource.loop = loop;
            musicSource.Play();
            Debug.Log("Playing Music: " + musicClips[index].name);
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PlaySFX(int index)
    {
        if (sfxClips == null || index < 0 || index >= sfxClips.Length)
            return;

        if(sfxSource.clip != sfxClips[index])
        {
            sfxSource.clip = sfxClips[index];
        }
        
        sfxSource.PlayOneShot(sfxClips[index]);
        Debug.Log("Playing SFX: " + sfxClips[index].name);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = Mathf.Clamp01(volume);
    }
}