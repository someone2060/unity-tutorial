using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource _audioSource;
    private float _volume;

    private void Awake()
    {
        Instance = this;
        
        _audioSource = GetComponent<AudioSource>();
        _volume = .3f;
    }

    public void ChangeVolume()
    {
        _volume += .1f;
        if (_volume > 1.0f) _volume = 0.0f;

        _audioSource.volume = _volume;
    }

    public float GetVolume() => _volume;
}
