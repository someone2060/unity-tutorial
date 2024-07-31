using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    private float _fryingProgress;

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

        FryingRecipeSO fryingRecipeSO = GetCuttingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
        // Player placing object that isn't CuttingRecipeSO.input
        if (!HasRecipeWithInput(fryingRecipeSO)) return;

        player.GetKitchenObject().SetKitchenObjectParent(this);
    }

    public override void InteractAlternate(Player player) { }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        return fryingRecipeSO is not null;
    }

    private bool HasRecipeWithInput(FryingRecipeSO cuttingRecipeSO)
    {
        return cuttingRecipeSO is not null;
    }

    private KitchenObjectSO GetOutputKitchenObject(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        
        if (fryingRecipeSO is not null) return fryingRecipeSO.output;
        return null;
    }

    private FryingRecipeSO GetCuttingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
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
