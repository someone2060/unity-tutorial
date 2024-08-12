using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsBindingUI : MonoBehaviour
{
    [SerializeField] private Button bindingButton;
    [SerializeField] private TextMeshProUGUI bindingText;
    [SerializeField] private Button resetButton;
    [SerializeField] private GameInput.Binding binding;
    [SerializeField] private Color rebindingTextColour;

    private Color _defaultTextColour;

    private void Awake()
    {
        _defaultTextColour = bindingText.color;
        
        bindingButton.onClick.AddListener(() =>
        {
            bindingText.color = rebindingTextColour;
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
