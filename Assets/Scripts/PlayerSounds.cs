using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player _player;
    private float _footstepTimer;
    private const float FootstepTimerMax = .1f;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _footstepTimer = 0.0f;
    }

    private void Update()
    {
        if (!_player.IsWalking()) return; // Not walking
        
        _footstepTimer -= Time.deltaTime;
        
        if (_footstepTimer >= 0) return; // Not enough time passed in _footstepTimer

        _footstepTimer = FootstepTimerMax;

        const float volume = 0.5f;
        SoundManager.Instance.PlayFootstepSound(_player.transform.position, volume);
    }
}
