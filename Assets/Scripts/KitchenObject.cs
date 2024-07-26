using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private ClearCounter _clearCounter;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (_clearCounter is not null)
        {
            _clearCounter.ClearKitchenObject();
        }
        
        _clearCounter = clearCounter;
        if (_clearCounter.HasKitchenObject())
        {
            Debug.LogError("Counter already has a KitchenObject!");
        }
        _clearCounter.SetKitchenObject(this);
        
        transform.parent = clearCounter.GetCounterTopPoint();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}