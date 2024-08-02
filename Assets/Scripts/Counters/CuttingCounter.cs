using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter, IHasProgress
{
    private enum State
    {
        Idle,
        Cutting,
        Cut
    }
    
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    
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
        if (!HasKitchenObject()) // Counter not holding something
        {
            if (!player.HasKitchenObject()) return; // Player not holding something

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
            return;
        }
        // Counter holding something

        if (!player.HasKitchenObject()) // Player not holding something
        {
            GetKitchenObject().SetKitchenObjectParent(player);

            _state = State.Idle;

            UpdateProgress(0.0f);
            return;
        }
        // Player holding something

        if (!player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)) return; // Player not holding a plate

        if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) return; // Unable to add ingredient to plate

        GetKitchenObject().DestroySelf();
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

    /**
     * Sends OnProgressChanged event with cutting progress.
     * @param progress is a range from 0-1, with 0 as no progress and 1 as full progress.
     */
    private void UpdateProgress(float progress)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
        {
            ProgressNormalized = progress
        });
    }
}
