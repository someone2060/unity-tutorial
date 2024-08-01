using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    private float _spawnPlateTimer;
    private const float SpawnPlateTimerMax = 4.0f;
    private int _platesSpawned;
    private const int PlatesSpawnedMax = 4;

    public void Awake()
    {
        _platesSpawned = 0;
    }

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        
        if (_spawnPlateTimer < SpawnPlateTimerMax) return; // Not enough time passed to spawn plate
        
        _spawnPlateTimer = 0.0f;
        
        if (_platesSpawned >= PlatesSpawnedMax) return; // Max plates spawned

        _platesSpawned++;
    }

    public override void Interact(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void InteractAlternate(Player player)
    {
        throw new System.NotImplementedException();
    }
}
