using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsRebindingUI : MonoBehaviour
{
    [SerializeField] private Button bindingButton;
    [SerializeField] private TextMeshProUGUI bindingText;
    [SerializeField] private Button resetButton;
    [SerializeField] private GameInput.Binding binding;

    private Color _defaultTextColour;
    private Color _highlightTextColour;

    private void Awake()
    {
        _defaultTextColour = bindingText.color;
        _highlightTextColour = new Color(210/255.0f, 175/255.0f, 0);
        
        bindingButton.onClick.AddListener(() =>
        {
            bindingText.color = _highlightTextColour;
            GameInput.Instance.RebindBinding(binding, () =>
            {
                bindingText.color = _defaultTextColour;
                UpdateVisual();
            });
        });
        
        resetButton.onClick.AddListener(() =>
        {
            GameInput.Instance.ResetBinding(binding, UpdateVisual);
        });
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        bindingText.text = GameInput.Instance.GetBindingText(binding);
    }
}
