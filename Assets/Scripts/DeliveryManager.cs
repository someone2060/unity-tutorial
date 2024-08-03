using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeListSO recipeListSO;
    
    private List<RecipeSO> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private const float SpawnRecipeTimerMax = 4.0f;
    private const int WaitingRecipesMax = 4;

    private void Awake()
    {
        _waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (_waitingRecipeSOList.Count >= WaitingRecipesMax) return; // Reached waiting max
        
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer > 0) return; // Not enough time elapsed

        _spawnRecipeTimer = SpawnRecipeTimerMax;

        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count-1)];
        Debug.Log(waitingRecipeSO.recipeName); //DEBUG
        _waitingRecipeSOList.Add(waitingRecipeSO);
    }
}
