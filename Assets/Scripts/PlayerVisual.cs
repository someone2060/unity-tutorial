using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string IsWalking = "IsWalking";

    [SerializeField] private Player player;
    
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash(IsWalking);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(Walking, player.IsWalking());
    }
}
