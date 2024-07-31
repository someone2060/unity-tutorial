using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    
    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;

    private void Update()
    {
        // Counter doesn't have KitchenObject
        if (!HasKitchenObject()) return;

        _fryingTimer += Time.deltaTime;

        _fryingRecipeSO = GetFryingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
        if (!HasRecipeWithInput(_fryingRecipeSO)) return;

        // Not fried
        if (_fryingTimer <= _fryingRecipeSO.fryingTimerMax) return;

        _fryingTimer = 0.0f;
        Debug.Log("Fried!");
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObject()) // Counter has KitchenObject
        {
            if (!player.HasKitchenObject()) // Player doesn't have KitchenObject
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            return;
        }

        if (!player.HasKitchenObject()) return; // Player doesn't have KitchenObject

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
        // Player placing object that isn't CuttingRecipeSO.input
        if (!HasRecipeWithInput(fryingRecipeSO)) return;

        player.GetKitchenObject().SetKitchenObjectParent(this);
    }

    public override void InteractAlternate(Player player) { }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(inputKitchenObjectSO);
        return fryingRecipeSO is not null;
    }

    private bool HasRecipeWithInput(FryingRecipeSO cuttingRecipeSO)
    {
        return cuttingRecipeSO is not null;
    }

    private KitchenObjectSO GetOutputKitchenObject(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(inputKitchenObjectSO);
        
        if (fryingRecipeSO is not null) return fryingRecipeSO.output;
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (inputKitchenObjectSO == fryingRecipeSO.input)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }
}
