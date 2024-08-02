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
        if (!HasKitchenObject()) // Counter not holding something
        {
            if (!player.HasKitchenObject()) return; // Player not holding something

            player.GetKitchenObject().SetKitchenObjectParent(this);
            return;
        }
        // Counter holding something

        if (!player.HasKitchenObject()) // Player not holding something
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            return;
        }
        // Player holding something

        if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)) // Player holding a plate
        {
            if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                return; // Unable to add ingredient to player's plate

            GetKitchenObject().DestroySelf();
            return;
        }
        // Player not holding a plate

        if (!GetKitchenObject().TryGetPlate(out plateKitchenObject)) return; // Counter not holding a plate

        if (!plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) 
            return; // Unable to add ingredient to counter's plate

        player.GetKitchenObject().DestroySelf();
    }
    
    public override void InteractAlternate(Player player) { }
}
