using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;

    private void Awake()
    {
        musicButton.onClick.AddListener((() =>
        {
            
        }));
        soundEffectsButton.onClick.AddListener((() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        }));
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10.0f);
    }
}
