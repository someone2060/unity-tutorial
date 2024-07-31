using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    public event EventHandler OnCut;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private float _cuttingProgress;
    private float _secondsToNextCut;
    private int _cuts;
    
    public override void Interact(Player player)
    {
        if (HasKitchenObject()) // Counter has KitchenObject
        {
            if (!player.HasKitchenObject()) // Player doesn't have KitchenObject
            {
                GetKitchenObject().SetKitchenObjectParent(player);
        
                // Send event
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs()
                {
                    ProgressNormalized = 0.0f
                });
            }
            return;
        }

        if (!player.HasKitchenObject()) return; // Player doesn't have KitchenObject

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
        // Player placing object that isn't CuttingRecipeSO.input
        if (!HasRecipeWithInput(cuttingRecipeSO)) return;

        // Move KitchenObject to counter, reset cutting progress
        player.GetKitchenObject().SetKitchenObjectParent(this);
        _cuttingProgress = 0.0f;
        _secondsToNextCut = (1.0f / cuttingRecipeSO.cutsNeeded) * cuttingRecipeSO.secondsToCut;
        _cuts = 0;
        
        // Send event
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs()
        {
            ProgressNormalized = 0.0f
        });
    }

    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject()) return;
        
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
        if (!HasRecipeWithInput(cuttingRecipeSO)) return;
        
        _cuttingProgress += Time.deltaTime;

        int currentCuts = (int)(_cuttingProgress / _secondsToNextCut);
        
        if (_cuts != currentCuts) // Update visuals
        {
            _cuts = currentCuts;
            
            // Send event
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs()
            {
                ProgressNormalized = (float)_cuts / cuttingRecipeSO.cutsNeeded
            });
            OnCut?.Invoke(this, EventArgs.Empty);
        }
            
        // Check if cutting finished
        if (_cuttingProgress < cuttingRecipeSO.secondsToCut) return;
        
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
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
