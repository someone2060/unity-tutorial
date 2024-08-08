using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button musicButton;
    [SerializeField] private Button soundEffectsButton;

    private void Awake()
    {
        musicButton.onClick.AddListener((() =>
        {
            
        }));
        soundEffectsButton.onClick.AddListener((() =>
        {
            
        }));
    }
}
