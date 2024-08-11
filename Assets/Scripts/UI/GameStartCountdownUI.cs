using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";
    private static readonly int NumberPopup = Animator.StringToHash(NUMBER_POPUP);

    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator _animator;
    private int _pCountdownNumber;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
            return;
        }

        Show();
    }

    private void Update()
    {
        var countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (_pCountdownNumber == countdownNumber) return;
        
        _pCountdownNumber = countdownNumber;
        _animator.SetTrigger(NumberPopup);
        SoundManager.Instance.PlayCountdownSound();
    }
    
    private void Show() => gameObject.SetActive(true);

    private void Hide() => gameObject.SetActive(false);
}
