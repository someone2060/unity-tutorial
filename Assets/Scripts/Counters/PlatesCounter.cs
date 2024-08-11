using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    
    private float _spawnPlateTimer;
    private const float SpawnPlateTimerMax = 4.0f;
    private int _platesSpawned;
    private const int PlatesSpawnedMax = 4;

    private void Awake()
    {
        _platesSpawned = 0;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return; // Not in GamePlaying phase
        if (_platesSpawned >= PlatesSpawnedMax) return; // Max plates spawned
        
        _spawnPlateTimer += Time.deltaTime;
        
        if (_spawnPlateTimer < SpawnPlateTimerMax) return; // Not enough time passed to spawn plate
        
        _spawnPlateTimer = 0.0f;

        _platesSpawned++;
        
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) return; // Player is holding a KitchenObject

        if (_platesSpawned <= 0) return; // No plates to give player

        _platesSpawned--;
        
        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
        
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }

    public override void InteractAlternate(Player player) { }
}
