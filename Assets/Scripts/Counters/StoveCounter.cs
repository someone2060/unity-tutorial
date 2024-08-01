using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter
{
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
                
                if (_fryingTimer <= _fryingRecipeSO.fryingTimerMax) return; // Not fried
                
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                _state = State.Fried;
                _burningTimer = 0.0f;
                _burningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                
                UpdateState(_state);
                break;
            case State.Fried:
                _burningTimer += Time.deltaTime;
                
                if (_burningTimer <= _burningRecipeSO.burningTimerMax) return; // Not fried
                
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);
                
                _state = State.Burned;
                
                UpdateState(_state);
                break;
            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObject()) // Counter has KitchenObject
        {
            if (!player.HasKitchenObject()) // Player doesn't have KitchenObject
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                _state = State.Idle;
                
                UpdateState(_state);
            }
            return;
        }

        if (!player.HasKitchenObject()) return; // Player doesn't have KitchenObject

        _fryingRecipeSO = GetFryingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
        // Player placing object that isn't CuttingRecipeSO.input
        if (!HasRecipeWithInput(_fryingRecipeSO)) return;

        player.GetKitchenObject().SetKitchenObjectParent(this);
        
        _state = State.Frying;
        _fryingTimer = 0.0f;
                
        UpdateState(_state);
    }

    public override void InteractAlternate(Player player) { }

    private void UpdateState(State state)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            State = state
        });
    }

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
}
