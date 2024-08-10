using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    
    [SerializeField] private Button backButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundEffectsText;

    [SerializeField] private OptionsRebindingUI moveUpOptionsRebindingUI;
    [SerializeField] private OptionsRebindingUI moveDownOptionsRebindingUI;
    [SerializeField] private OptionsRebindingUI moveLeftOptionsRebindingUI;
    [SerializeField] private OptionsRebindingUI moveRightOptionsRebindingUI;
    [SerializeField] private OptionsRebindingUI interactOptionsRebindingUI;
    [SerializeField] private OptionsRebindingUI interactAlternateOptionsRebindingUI;
    [SerializeField] private OptionsRebindingUI pauseOptionsRebindingUI;

    private void Awake()
    {
        Instance = this;
        
        backButton.onClick.AddListener((() =>
        {
            GamePauseUI.Instance.Show();
            Hide();
        }));
        
        musicSlider.onValueChanged.AddListener(value =>
        {
            MusicManager.Instance.SetVolume(value);
            UpdateVisual();
        });
        soundEffectsSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.SetVolume(value);
            UpdateVisual();
        });
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
        // Music, sound effects
        musicSlider.value = MusicManager.Instance.GetVolume();
        soundEffectsSlider.value = SoundManager.Instance.GetVolume();
        musicText.text = $"Music: {Mathf.Round(MusicManager.Instance.GetVolume() * 100.0f)}%";
        soundEffectsText.text = $"Sound Effects: {Mathf.Round(SoundManager.Instance.GetVolume() * 100.0f)}%";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
