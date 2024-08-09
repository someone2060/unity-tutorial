using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource _audioSource;
    private bool _playSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playSound = false;
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        _audioSource.Pause();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        if (!_playSound) return;
        
        _audioSource.Play();
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        _playSound = e.State is StoveCounter.State.Frying or StoveCounter.State.Fried;

        if (_playSound)
        {
            _audioSource.Play();
            return;
        }
        
        _audioSource.Pause();
    }
}
