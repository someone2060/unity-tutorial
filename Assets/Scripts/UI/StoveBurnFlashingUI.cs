using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    private static readonly int IsFlashing = Animator.StringToHash(IS_FLASHING);

    [SerializeField] private StoveCounter stoveCounter;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        
        _animator.SetBool(IsFlashing, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        const float burnShowProgressAmount = 0.5f;
        var flash = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;

        Debug.Log(flash);
        _animator.SetBool(IsFlashing, flash);
    }
}
