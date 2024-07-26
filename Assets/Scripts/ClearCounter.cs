using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject _kitchenObject;
    
    public void Interact()
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.localPosition = Vector3.zero;
            
            _kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        }
    }
}
