using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaced;
    
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject _kitchenObject;
    
    public abstract void Interact(Player player);
    public abstract void InteractAlternate(Player player);

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject is null) return;
        
        OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
    }

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject is not null;
}
