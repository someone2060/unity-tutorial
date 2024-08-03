using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    IHasProgress _hasProgress;
    
    private void Start()
    {
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (_hasProgress is null)
        {
            Debug.LogError($"Game Object {hasProgressGameObject} does not have a component that implements IHasProgress!");
        }
        
        _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        
        barImage.fillAmount = 0.0f;
        
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.ProgressNormalized;

        if (e.ProgressNormalized == 0f || Math.Abs(e.ProgressNormalized - 1f) < 0.01)
        {
            Hide();
        } else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
