using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    public static GamePauseUI Instance { get; private set; }
    
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        Instance = this;
        
        resumeButton.onClick.AddListener((() =>
        {
            GameManager.Instance.TogglePauseGame();
        }));
        mainMenuButton.onClick.AddListener((() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        }));
        optionsButton.onClick.AddListener((() =>
        {
            OptionsUI.Instance.Show(Show);
            Hide();
        }));
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
