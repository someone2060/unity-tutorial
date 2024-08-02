using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded; 
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }
    
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    
    private List<KitchenObjectSO> _kitchenObjectSOList;

    private void Awake()
    {
        _kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) return false; // Not valid ingredient
        
        if (_kitchenObjectSOList.Contains(kitchenObjectSO)) return false; // Already has type
        
        _kitchenObjectSOList.Add(kitchenObjectSO);
        return true;
    }

    private void InvokeOnIngredientAdded(KitchenObjectSO kitchenObjectSO)
    {
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs()
        {
            KitchenObjectSO = kitchenObjectSO
        });
    }
}
