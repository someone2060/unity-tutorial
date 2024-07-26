using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    
    private KitchenObject _kitchenObject;

    public override void Interact(Player player)
    {
        if (_kitchenObject is null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            return;
        }
        
        _kitchenObject.SetKitchenObjectParent(player);
    }

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject is not null;
}
