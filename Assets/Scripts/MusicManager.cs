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

    public void ChangeVolume()
    {
        _volume += .1f;
        if (_volume > 1.05f) _volume = 0.0f;

        SetVolume(_volume);
        
        PlayerPrefs.SetFloat(PlayerPrefsMusicVolume, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() => _volume;

    private void SetVolume(float volume)
    {
        _volume = volume;
        _audioSource.volume = _volume;
    }
}
