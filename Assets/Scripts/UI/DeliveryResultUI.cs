using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";
    
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColour;
    [SerializeField] private Color failedColour;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;

    private Animator _animator;
    private static readonly int Popup = Animator.StringToHash(POPUP);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        
        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Popup);

        backgroundImage.color = successColour;
        iconImage.sprite = successSprite;
        messageText.text = "Delivery\nSuccess!";
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Popup);

        backgroundImage.color = failedColour;
        iconImage.sprite = failedSprite;
        messageText.text = "Delivery\nFailed...";
    }
}
