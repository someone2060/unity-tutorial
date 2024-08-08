using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    
    [SerializeField] private Button musicButton;
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundEffectsText;

    private void Awake()
    {
        Instance = this;
        
        musicButton.onClick.AddListener((() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        }));
        soundEffectsButton.onClick.AddListener((() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        }));
        backButton.onClick.AddListener((() =>
        {
            GamePauseUI.Instance.Show();
            Hide();
        }));
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10.0f);
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10.0f);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
