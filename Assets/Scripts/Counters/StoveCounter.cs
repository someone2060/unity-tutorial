using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State State;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State _state;
    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private float _burningTimer;
    private BurningRecipeSO _burningRecipeSO;

    private void Awake()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        if (!HasKitchenObject()) return; // Counter doesn't have KitchenObject
        
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Frying:
                _fryingTimer += Time.deltaTime;
                InvokeOnProgressChanged(_fryingTimer / _fryingRecipeSO.fryingTimerMax);
                
                if (_fryingTimer <= _fryingRecipeSO.fryingTimerMax) return; // Not fried
                
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                _state = State.Fried;
                _burningTimer = 0.0f;
                _burningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                
                InvokeOnStateChanged(_state);
                break;
            case State.Fried:
                _burningTimer += Time.deltaTime;
                InvokeOnProgressChanged(_burningTimer / _burningRecipeSO.burningTimerMax);
                
                if (_burningTimer <= _burningRecipeSO.burningTimerMax) return; // Not burned
                
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);
                
                _state = State.Burned;
                
                InvokeOnStateChanged(_state);
                InvokeOnProgressChanged(0.0f);
                break;
            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) // Counter not holding something
        {
            if (!player.HasKitchenObject()) return; // Player not holding something

            _fryingRecipeSO = GetFryingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
            
            if (!HasRecipeWithInput(_fryingRecipeSO)) return; // Player placing object that isn't CuttingRecipeSO.input

            player.GetKitchenObject().SetKitchenObjectParent(this);

            _state = State.Frying;
            _fryingTimer = 0.0f;

            InvokeOnStateChanged(_state);
            InvokeOnProgressChanged(0.0f);
            return;
        }
        // Counter holding something

        if (!player.HasKitchenObject()) // Player not holding something
        {
            GetKitchenObject().SetKitchenObjectParent(player);

            _state = State.Idle;

            InvokeOnStateChanged(_state);
            InvokeOnProgressChanged(0.0f);
            return;
        }
        // Player holding something

        if (!player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)) return; // Player not holding a plate

        if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) return; // Unable to add ingredient to plate

        GetKitchenObject().DestroySelf();

        _state = State.Idle;

        InvokeOnStateChanged(_state);
        InvokeOnProgressChanged(0.0f);
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

    private BurningRecipeSO GetBurningRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (inputKitchenObjectSO == burningRecipeSO.input)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    private void InvokeOnStateChanged(State state)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            State = state
        });
    }

    /**
     * Sends OnProgressChanged event with cutting progress.
     * @param progress is a range from 0-1, with 0 as no progress and 1 as full progress.
     */
    private void InvokeOnProgressChanged(float progress)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
        {
            ProgressNormalized = progress
        });
    }

    public bool IsFried() => _state == State.Fried;
}
