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
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private Transform pressToRebindKeyTransform;

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
        
        moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveUp) );
        moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveDown) );
        moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveLeft) );
        moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveRight) );
        interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact) );
        interactAlternateButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternate) );
        pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause) );
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        
        HidePressToRebindKey();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        // Music, sound effets
        musicSlider.value = MusicManager.Instance.GetVolume();
        soundEffectsSlider.value = SoundManager.Instance.GetVolume();
        musicText.text = $"Music: {Mathf.Round(MusicManager.Instance.GetVolume() * 100.0f)}%";
        soundEffectsText.text = $"Sound Effects: {Mathf.Round(SoundManager.Instance.GetVolume() * 100.0f)}%";
        // Keybinds
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
