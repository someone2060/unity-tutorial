using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource _audioSource;
    private bool _playSizzleSound;
    private float _warningSoundTimer;
    private bool _playWarningSound;
    private const float WarningSoundTimerMax = 0.2f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playSizzleSound = false;
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    }

    private void Update()
    {
        if (!_playWarningSound) return;
        
        _warningSoundTimer -= Time.deltaTime;
        
        if (_warningSoundTimer > 0) return;

        _warningSoundTimer = WarningSoundTimerMax;
        SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        _audioSource.Pause();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        if (!_playSizzleSound) return;
        
        _audioSource.Play();
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        _playSizzleSound = e.State is StoveCounter.State.Frying or StoveCounter.State.Fried;

        if (_playSizzleSound)
        {
            _audioSource.Play();
            return;
        }
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        const float burnShowProgressAmount = 0.5f;
        _playWarningSound = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;
    }
}
