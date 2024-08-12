using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _state;
    private float _countdownToStartTimer = 3.0f;
    private float _gamePlayingTimer;
    private const float GamePlayingTimerMax = 150.0f;
    private bool _isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        
        _state = State.WaitingToStart;
        _gamePlayingTimer = GamePlayingTimerMax;
    }

    private void Start()
    {
        GameInput.Instance.OnPausePerformed += GameInput_OnPausePerformed;
        GameInput.Instance.OnInteractPerformed += GameInput_OnInteractPerformed;
    }

    private void GameInput_OnInteractPerformed(object sender, EventArgs e)
    {
        if (_state != State.WaitingToStart) return;
        
        _state = State.CountdownToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnPausePerformed(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (_state)
        {
            default:
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                
                if (_countdownToStartTimer >= 0) break;

                _state = State.GamePlaying;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                
                if (_gamePlayingTimer >= 0) break;

                _state = State.GameOver;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying() => _state == State.GamePlaying;

    public bool IsGamePaused() => _isGamePaused;

    public bool IsCountdownToStartActive() => _state == State.CountdownToStart;

    public bool IsGameOver() => _state == State.GameOver;

    public float GetCountdownToStartTimer() => _countdownToStartTimer;

    public float GetGamePlayingTimerNormalized() => 1 - (_gamePlayingTimer / GamePlayingTimerMax);
    
    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0.0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            return;
        }

        Time.timeScale = 1.0f;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
}
