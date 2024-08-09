using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const string PlayerPrefsMusicVolume = "MusicVolume";

    private AudioSource _audioSource;
    private float _volume;

    private void Awake()
    {
        Instance = this;
        
        _audioSource = GetComponent<AudioSource>();
        
        SetVolume(PlayerPrefs.GetFloat(PlayerPrefsMusicVolume, .3f));
    }

    public float GetVolume() => _volume;

    public void SetVolume(float volume)
    {
        if (volume > 1) volume = 1.0f;
        if (volume < 0) volume = 0.0f;
        _volume = volume;
        _audioSource.volume = _volume;
        
        PlayerPrefs.SetFloat(PlayerPrefsMusicVolume, _volume);
        PlayerPrefs.Save();
    }
}
