using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int _cuttingProgress;
    
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

        if (!HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) return; // Player placing object that isn't CuttingRecipeSO.input

        _cuttingProgress = 0;
        player.GetKitchenObject().SetKitchenObjectParent(this);
    }

    public override void InteractAlternate(Player player)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
        if (HasKitchenObject() && HasRecipeWithInput(cuttingRecipeSO))
        {
            _cuttingProgress++;
            
            if (_cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        return cuttingRecipeSO is not null;
    }

    private bool HasRecipeWithInput(CuttingRecipeSO cuttingRecipeSO)
    {
        return cuttingRecipeSO is not null;
    }

    private KitchenObjectSO GetOutputKitchenObject(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(inputKitchenObjectSO);
        
        if (cuttingRecipeSO is not null) return cuttingRecipeSO.output;
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (inputKitchenObjectSO == cuttingRecipeSO.input)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}
