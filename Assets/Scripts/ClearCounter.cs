using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;
    
    private KitchenObject _kitchenObject;

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject is not null;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T))
        {
            if (_kitchenObject is null)
            {
                return;
            }
            
            _kitchenObject.SetKitchenObjectParent(secondClearCounter);
        }
    }

    public void Interact(Player player)
    {
        if (_kitchenObject is null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            return;
        }
        
        //_kitchenObject.SetKitchenObjectParent(player);
        Debug.Log(_kitchenObject.GetKitchenObjectParent());
    }
}
