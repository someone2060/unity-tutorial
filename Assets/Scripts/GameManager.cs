using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _state;
    private float _waitingToStartTimer = 1.0f;
    private float _countdownToStartTimer = 3.0f;
    private float _gamePlayingTimer;
    private const float GamePlayingTimerMax = 10.0f;
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
    }

    private void GameInput_OnPausePerformed(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                
                if (_waitingToStartTimer >= 0) break;

                _state = State.CountdownToStart;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
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

    public bool IsCountdownToStartActive() => _state == State.CountdownToStart;

    public bool IsGameOver() => _state == State.GameOver;

    public float GetCountdownToStartTimer() => _countdownToStartTimer;

    public float GetGamePlayingTimerNormalized() => 1 - (_gamePlayingTimer / GamePlayingTimerMax);
    
    private void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0.0f;
            return;
        }

        Time.timeScale = 1.0f;
    }
}
