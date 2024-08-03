using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    
    public static DeliveryManager Instance { get; private set; }
    
    [SerializeField] private RecipeListSO recipeListSO;
    
    private List<RecipeSO> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private const float SpawnRecipeTimerMax = 4.0f;
    private const int WaitingRecipesMax = 4;

    private void Awake()
    {
        Instance = this;
        _waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (_waitingRecipeSOList.Count >= WaitingRecipesMax) return; // Reached waiting max
        
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer > 0) return; // Not enough time elapsed

        _spawnRecipeTimer = SpawnRecipeTimerMax;

        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
        _waitingRecipeSOList.Add(waitingRecipeSO);
        
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    public bool TryDeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        var plateAmountIngredients = plateKitchenObject.GetKitchenObjectSOList().Count;

        for (var i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            var waitingRecipeSO = _waitingRecipeSOList[i];
            if (plateAmountIngredients != waitingRecipeSO.kitchenObjectSOList.Count) continue; // Not same number of ingredients

            var validRecipe = waitingRecipeSO.kitchenObjectSOList.All(
                plateKitchenObjectSO => waitingRecipeSO.kitchenObjectSOList.Contains(plateKitchenObjectSO));

            if (!validRecipe) continue; // Not all ingredients matched

            _waitingRecipeSOList.RemoveAt(i);

            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            return true;
        }
        
        return false;
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return _waitingRecipeSOList;
    }
}
