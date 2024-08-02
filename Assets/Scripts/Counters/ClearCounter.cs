using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if (HasKitchenObject()) // Counter has a KitchenObject
        {
            if (!player.HasKitchenObject()) // Player doesn't have a KitchenObject
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                return;
            }

            // Player has a KitchenObject
            if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return; // Player not holding a plate

            if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) return; // Unable to add ingredient to plate
            
            GetKitchenObject().DestroySelf();
            return;
        } // Counter doesn't have a KitchenObject

        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
    }
    
    public override void InteractAlternate(Player player) { }
}
