using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Cutting,
        Cut
    }
    
    public event EventHandler OnCut;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private State _state;
    private float _cuttingTimer;
    private float _secondsToNextCut;
    private int _cuts;
    private CuttingRecipeSO _cuttingRecipeSO;

    private void Awake()
    {
        _state = State.Idle;
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObject()) // Counter has KitchenObject
        {
            if (!player.HasKitchenObject()) // Player doesn't have KitchenObject
            {
                GetKitchenObject().SetKitchenObjectParent(player);
        
                _state = State.Idle;

                UpdateProgress(0.0f);
            }
            return;
        }

        if (!player.HasKitchenObject()) return; // Player doesn't have KitchenObject

        _cuttingRecipeSO = GetCuttingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
        // Player placing object that isn't CuttingRecipeSO.input
        if (!HasRecipeWithInput(_cuttingRecipeSO)) return;

        // Move KitchenObject to counter, reset cutting progress
        player.GetKitchenObject().SetKitchenObjectParent(this);

        _state = State.Cutting;
        _cuttingTimer = 0.0f;
        _secondsToNextCut = (1.0f / _cuttingRecipeSO.cutsNeeded) * _cuttingRecipeSO.secondsToCut;
        _cuts = 0;
        
        UpdateProgress(0.0f);
    }

    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject()) return;

        switch (_state)
        {
            case State.Idle:
                break;
            case State.Cutting:
                _cuttingTimer += Time.deltaTime;

                var currentCuts = (int)(_cuttingTimer / _secondsToNextCut);
        
                if (_cuts != currentCuts) // Update visuals
                {
                    _cuts = currentCuts;
            
                    UpdateProgress((float)_cuts / _cuttingRecipeSO.cutsNeeded);
                    OnCut?.Invoke(this, EventArgs.Empty);
                }
            
                // Cutting not finished
                if (_cuttingTimer < _cuttingRecipeSO.secondsToCut) return;
        
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(_cuttingRecipeSO.output, this);

                _state = State.Cut;
                break;
            case State.Cut:
                break;
        }
    }

    /**
     * Sends OnProgressChanged event with cutting progress.
     * @param progress is a range from 0-1, with 0 as no progress and 1 as full progress.
     */
    private void UpdateProgress(float progress)
    {
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs()
        {
            ProgressNormalized = progress
        });
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
