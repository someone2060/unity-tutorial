using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;
    
    private KitchenObject _kitchenObject;

    public Transform GetCounterTopPoint()
    {
        return counterTopPoint;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject is not null;
    }

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T))
        {
            if (_kitchenObject is null)
            {
                return;
            }
            
            _kitchenObject.SetClearCounter(secondClearCounter);
        }
    }

    public void Interact()
    {
        if (_kitchenObject is null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetClearCounter(this);
            return;
        }
        
        Debug.Log(_kitchenObject.GetClearCounter());
    }
}
