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
        _clearCounter = clearCounter;
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}